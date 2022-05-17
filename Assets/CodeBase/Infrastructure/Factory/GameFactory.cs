using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.GameEntities.Villager;
using CodeBase.GameResources;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Formation;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private readonly IAssetProviderService _assets;
        private readonly IStaticDataService _staticData;
        private readonly IWindowService _windowService;
        private readonly IFormationService _formationService;
        
        private GameObject _heroGameObject;
        public GameObject HeroGameObject => _heroGameObject;

        public GameFactory(
            IAssetProviderService assets,
            IStaticDataService staticData,
            IWindowService windowService,
            IFormationService formationService)
        {
            _formationService = formationService;
            _assets = assets;
            _staticData = staticData;
            _windowService = windowService;
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
        private GameObject InstantiateRegistred(string prefab)
        {
            GameObject gameObject = _assets.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistred(string prefab, Vector3 position)
        {
            GameObject gameObject = _assets.Instantiate(prefab, position, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
        
        private GameObject InstantiateRegistred(string prefab, Vector3 position, Vector3 rotation)
        {
            GameObject gameObject = _assets.Instantiate(prefab, position, Quaternion.Euler(rotation));
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        public GameObject CreatePlayerObject(Vector3 at)
        {
            _heroGameObject = InstantiateRegistred(AssetPath.HeroPath, at, new Vector3(0, 230, 0));
            return _heroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistred(AssetPath.HudPath);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(_windowService);

            return hud;
        }

        public Unit CreateVillager(Vector3 at, GameEntities.Village village)
        {
            Unit unit = _assets.Instantiate(AssetPath.VillagerPath, at).GetComponent<Unit>();
            unit.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            unit.Construct(_formationService, _staticData, village);
            return unit;
        }

        public void CreateSavedArmy(PlayerProgress progress, string levelName)
        {
            ProgressWriters.Add(_formationService);
            
            for (int i = 0; i < progress.DataOnLevel[levelName].UnitsLevels.Length - 1; i++)
            {
                Unit unit = _assets.Instantiate(AssetPath.VillagerPath, _heroGameObject.transform.position + Vector3.right * i * 0.1f).GetComponent<Unit>();
                unit.CreateInArmy(_formationService, _staticData, progress.DataOnLevel[levelName].UnitsLevels[i]);
            }
        }
        public GameObject CreateVillage(Vector3 at) => 
            InstantiateRegistred(AssetPath.VillagePath, at);

        public GameObject CreateGates(Vector3 at) => 
            InstantiateRegistred(AssetPath.GatesPath, at);

        public GameObject CreateCastle(Vector3 at, int castleNumber) => 
            InstantiateRegistred(AssetPath.CastlePath + castleNumber, at);

        public Collectible CreateCoin(Vector3 at) => 
            _assets.Instantiate(AssetPath.CoinPath, at).GetComponent<Collectible>();

        public GameObject CreateGround() => 
            _assets.Instantiate(AssetPath.GroundPath);

        public GameObject CreateDragon(DragonSpawnInfo info) => 
            _assets.Instantiate(AssetPath.DragonPath + info.DragonName, info.DragonPosition, Quaternion.Euler(0, info.DragonRotation, 0));

        public GameObject CreateBlock(Vector3 at) => 
            _assets.Instantiate(AssetPath.BlockPath, at, Quaternion.identity);

        public GameObject CreateGameManager() =>
            _assets.Instantiate(AssetPath.GameManagerPath);

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
    }
}