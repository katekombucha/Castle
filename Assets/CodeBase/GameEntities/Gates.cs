using System;
using CodeBase.Data;
using CodeBase.Equipment;
using CodeBase.GameEntities.Castle;
using CodeBase.GameResources;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Formation;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Pointers;
using UnityEngine;

namespace CodeBase.GameEntities
{
    public class Gates : MonoBehaviour, IUpgradablePlace, ISavedProgress
    {
        public GameObject PortalObject;
        public GameObject[] GateModels;
        public Action OnGatesUpgrade;

        private IStaticDataService _staticDataService;
        private IPointersService _pointersService;
        private IFormationService _formationService;
        private int _currentLevel;
        private int _maxLevel;
        private Pointer _unitsUpgradePointer;
        private Pointer _gatesUpgradePointer;
        private ResourceStack _resourceStack;
        private int _resourceShortage = 20;
        private readonly int _resourceToUpgrade = 20;

        public void Construct(ResourceStack resourceStack, IStaticDataService staticDataService,
            IPointersService pointersService, IFormationService formationService)
        {
            _formationService = formationService;
            _resourceStack = resourceStack;
            _staticDataService = staticDataService;
            _pointersService = pointersService;
        }

        private void Start()
        {
            _maxLevel = _staticDataService.UnitMaxLevel();
            _resourceStack.OnResourcesChanged += UpgradeGatesPointerEnable;
            _formationService.OnUpgradableExist += UpgradeArmyPointerEnable;
            _gatesUpgradePointer = AddPointer();
            _unitsUpgradePointer = AddPointer();
        }

        private Pointer AddPointer()
        {
            Pointer pointer = _pointersService.CreatePointer(transform.position);
            pointer.gameObject.SetActive(false);
            return pointer;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<CharacterLogic>())
            {
                other.GetComponent<CharacterLogic>().UnitEquipment.Upgrade(_currentLevel, true);
                return;
            }

            if (other.GetComponent<UnitEquipment>())
            {
                UnitEquipment equipment = other.GetComponent<UnitEquipment>();
                equipment.Upgrade(_currentLevel, true);
                _formationService.UpdateArmyLevel();

                if ((int)_formationService.ArmyLevel == _currentLevel)
                {
                    _unitsUpgradePointer.gameObject.SetActive(false);
                }
            }
        }

        public void Upgrade(int level)
        {
            if(level != 0)
                OnGatesUpgrade?.Invoke();
            
            GateModels[_currentLevel].SetActive(false);
            _currentLevel += level;
            GateModels[_currentLevel].SetActive(true);

            if(_currentLevel > 0)
                PortalObject.SetActive(true);
        }

        public bool CheckIfMaxLevel() => 
            _currentLevel == _maxLevel;

        private void UpgradeGatesPointerEnable(int coins) => 
            _gatesUpgradePointer.gameObject.SetActive(coins >= GetUpgradeCost());
        private void UpgradeArmyPointerEnable() => 
            _unitsUpgradePointer.gameObject.SetActive(true);

        public int GetUpgradeCost() => 
            (_currentLevel+1) * _resourceToUpgrade;

        public int ResourceShortage
        {
            get => _resourceShortage;
            set => _resourceShortage = value;
        }
        
        private void OnDisable()
        {
            _resourceStack.OnResourcesChanged -= UpgradeGatesPointerEnable;
            _formationService.OnUpgradableExist -= UpgradeArmyPointerEnable;
        }

        public void UpdateProgress(PlayerProgress progress, string levelName)
        {
            progress.DataOnLevel[levelName].GatesLevel = _currentLevel;
            progress.DataOnLevel[levelName].GatesUpgradeShortage = _resourceShortage;
        }

        public void LoadProgress(PlayerProgress progress, string levelName)
        {
            Upgrade(progress.DataOnLevel[levelName].GatesLevel);
            _resourceShortage = progress.DataOnLevel[levelName].GatesUpgradeShortage == 0 ?
                GetUpgradeCost() : progress.DataOnLevel[levelName].GatesUpgradeShortage;
        }
    }
}
