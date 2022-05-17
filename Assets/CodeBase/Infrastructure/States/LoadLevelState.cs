using System.Collections.Generic;
using Cinemachine;
using CodeBase.CameraLogic;
using CodeBase.GameEntities;
using CodeBase.GameEntities.Dragon;
using CodeBase.Hero;
using CodeBase.GameEntities.Castle;
using CodeBase.GameResources;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.Formation;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Pointers;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;
        private readonly IPointersService _pointersService;
        private IFormationService _formationService;
        private ISaveLoadService _saveLoadService;
        private LevelStaticData _levelData;
        private string _sceneName;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData,
            IUIFactory uiFactory, IPointersService pointersService, IFormationService formationService,
            ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _formationService = formationService;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
            _pointersService = pointersService;
        }

        public void Enter(string sceneName)
        {
            _sceneName = sceneName;
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
            
            SendEvent();
        }

        private  void SendEvent()
        {
            var data = new Dictionary<string, object>()
            {
                {"level_name", _sceneName}
            };
      
            AppMetrica.Instance.ReportEvent("level_start", data);
            AppMetrica.Instance.SendEventsBuffer();
        }
        
        public void Exit() { }

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();
            
            _saveLoadService.SaveNewLevel(_sceneName);
            _gameStateMachine.Enter<GameLoopState, string>(_sceneName);
        }

        private void InitUIRoot() =>
            _uiFactory.CreateUIRoot();

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress, _sceneName);
            }
            
            _gameFactory.CreateSavedArmy(_progressService.Progress, _sceneName);
        }

        private void InitGameWorld()
        {
            _levelData = LevelStaticData();
            ResourcesData resourcesData = new ResourcesData();
            CharacterLogic character = InitCharacter(_levelData, resourcesData);
            Gates gates = CreateGates(_levelData, character);

            InitHud(resourcesData);
            InitVillages(_levelData);
            CreateCastle(_levelData);
            CreateDragon(_levelData, character);
            CreateGameManager();

            new HintMessages(_uiFactory, gates, character, _progressService.Progress, _saveLoadService);
            
            if(_levelData.TutorialMessages.Length == 0)
                CameraFollow(character.gameObject);
            else
                CreateTutorial(character.gameObject);

            _formationService.Clear();
        }

        private void CreateGameManager() => 
            _gameFactory.CreateGameManager().GetComponent<GameManager>().Construct(_saveLoadService);

        private void CreateCastle(LevelStaticData levelData)
        {
            GameObject castle = _gameFactory.CreateCastle(levelData.CastlePosition, levelData.CastleNumber);
            castle.GetComponent<Castle>()
                .Construct(levelData.StartingNumberOfCitizens, _formationService, _saveLoadService, _pointersService, levelData.CoinsOffset);
        }

        private CharacterLogic InitCharacter(LevelStaticData levelData, ResourcesData resourcesData)
        {
            CharacterLogic character =
                _gameFactory.CreatePlayerObject(levelData.InitialHeroPosition).GetComponent<CharacterLogic>();
            character.Construct(_formationService, _staticData);
            character.ResourceStack.Construct(resourcesData, _gameFactory);
            return character;
        }

        private void CreateDragon(LevelStaticData levelData, CharacterLogic character)
        {
            if(levelData.DragonInfo.DragonPosition == Vector3.zero)
                return;
            
            CharacterBlock block = _gameFactory.CreateBlock(levelData.DragonBlockPosition)
                .GetComponent<CharacterBlock>();
            block.Construct(_formationService, new Vector3(2, 6, 4));
            GameObject dragon = _gameFactory.CreateDragon(levelData.DragonInfo);
            dragon.GetComponent<DragonController>().Construct(_formationService, character, block, levelData.DragonInfo);
        }

        private Gates CreateGates(LevelStaticData levelData, CharacterLogic characterLogic)
        {
            if(levelData.GatesPosition == Vector3.zero)
                return null;
            
            Gates gates = _gameFactory.CreateGates(levelData.GatesPosition).GetComponent<Gates>();
            gates.Construct(characterLogic.ResourceStack, _staticData, _pointersService, _formationService);
            return gates;
        }

        private void InitVillages(LevelStaticData levelData)
        {
            foreach (VillageData villageData in levelData.Villages)
            {
                GameObject village = _gameFactory.CreateVillage(villageData.Position);
                village.GetComponent<GameEntities.Village>().Construct(_gameFactory, _pointersService, _saveLoadService, villageData.Id);
            }
        }

        private LevelStaticData LevelStaticData() =>
            _staticData.ForLevel(_sceneName);

        private void InitHud(ResourcesData resourcesData) =>
            _uiFactory.CreateHud(resourcesData);

        private void CameraFollow(GameObject hero)
        {
            CinemachineSwitcher switcher = Object.FindObjectOfType<CinemachineSwitcher>();
                        
            foreach (CinemachineVirtualCamera camera in switcher.Cameras)
            {
                camera.Follow = hero.transform;
            }
            
            switcher.ChangeCamera();
        }

        private void CreateTutorial(GameObject hero)
        {
            CinemachineSwitcher switcher = Object.FindObjectOfType<CinemachineSwitcher>();
            
            foreach (CinemachineVirtualCamera camera in switcher.Cameras)
            {
                camera.Follow = hero.transform;
            }
            
            if (_progressService.Progress.DataOnLevel[_sceneName].TutorialComplete)
            {
                switcher.ChangeCamera();
                return;
            } 
            
            BaseWindow tutorialWindow = _uiFactory.CreateTutorial(_sceneName, _levelData);
            switcher.Subscribe(tutorialWindow);
            _progressService.Progress.DataOnLevel[_sceneName].TutorialComplete = true;

        }
    }
}