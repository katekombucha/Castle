using System.Collections;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.GameEntities.Villager;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Pointers;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.GameEntities
{
    public class Village : MonoBehaviour, IUpgradablePlace, ISavedProgress
    {
        public float SecondsToRespawn = 40f;
        public Transform[] GardenBeds;
        public GameObject TownHall;
        public Timer Timer;
        public MeshFilter HouseMeshFilter;
        public Mesh[] Houses;
        public MMFeedbacks ReleaseFeedback;
        public GameObject Chimney;
        
        private List<Unit> _villagers = new List<Unit>();
        private IPointersService _pointerService;
        private IGameFactory _gameFactory;
        private ISaveLoadService _saveLoadService;

        private int _resourceToUpgrade = 5;
        private int _resourceShortage;
        private int _currentLevel;
        private int _maxLevel = 2;
        private Coroutine _respawnCoroutine;
        private int _id;

        public void Construct(IGameFactory gameFactory, IPointersService pointersService, ISaveLoadService saveLoadService, int id)
        {
            _saveLoadService = saveLoadService;
            _pointerService = pointersService;
            _gameFactory = gameFactory;
            _id = id;
        }

        public void ReleaseVillager(Unit unit)
        {
            ReleaseFeedback.PlayFeedbacks();
            _villagers.Remove(unit);

            if (_villagers.Count == 0)
                AllVillagersGone();
        }

        private void AllVillagersGone()
        {
            _villagers = new List<Unit>();
            _pointerService.DeleteVillagePointer(TownHall.transform.position);
            Timer.EnableTimer(SecondsToRespawn);
            _respawnCoroutine = StartCoroutine(RespawnDelay());
        }

        private IEnumerator RespawnDelay()
        {
            yield return new WaitForSeconds(1);
            SaveProgress();
            yield return new WaitForSeconds(SecondsToRespawn-1);
            SpawnVillagers();
        }
        
        private void SpawnVillagers()
        {
            foreach (Transform gardenBed in GardenBeds)
            {
               Unit unit = _gameFactory.CreateVillager(gardenBed.position, this);
               _villagers.Add(unit);
            }

            _pointerService.AddVillagePointer(TownHall.transform.position);
        }

        public bool CheckIfMaxLevel() => 
            _currentLevel == _maxLevel;

        public int GetUpgradeCost() => 
            (_currentLevel+1) * _resourceToUpgrade;

        public void Upgrade(int level)
        {
            SecondsToRespawn = SecondsToRespawn - level*10;
            _currentLevel += level;
            RespawnImmediately();

            switch (_currentLevel)
            {
                case 1:
                    HouseMeshFilter.sharedMesh = Houses[1];
                    break;
                case 2:
                    HouseMeshFilter.sharedMesh = Houses[1];
                    Chimney.SetActive(true);
                    break;
            }
        }

        public int ResourceShortage
        {
            get => _resourceShortage;
            set => _resourceShortage = value;
        }

        private void RespawnImmediately()
        {
            if (_villagers.Count > 0)
                return;
            if(_respawnCoroutine != null)
                StopCoroutine(_respawnCoroutine);
            
            Timer.StopTimer();
            SpawnVillagers();
        }
        
        private void SaveProgress() => 
            _saveLoadService.SaveProgress();

        public void LoadProgress(PlayerProgress progress, string sceneName)
        {
            VillageData data = progress.DataOnLevel[sceneName].VillageData[_id];
            Upgrade(data.Level);
            _resourceShortage = data.ResourceShortage == 0 ? GetUpgradeCost() : data.ResourceShortage;
        }

        public void UpdateProgress(PlayerProgress progress, string sceneName) => 
            progress.DataOnLevel[sceneName].VillageData[_id].SaveUpgradeData(_currentLevel, _resourceShortage);
    }
}