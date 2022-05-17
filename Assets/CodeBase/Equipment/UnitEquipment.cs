using System.Linq;
using CodeBase.StaticData;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.Equipment
{
    public class UnitEquipment : MonoBehaviour
    {
        public EquipmentItem[] EquipmentMeshes;
        public MMFeedbacks UpgradeFeedbacks;
        [SerializeField] private MeshFilter _weapon;
        [SerializeField] private MeshFilter _shield;
        [SerializeField] private bool _isHero;

        private IStaticDataService _staticDataService;
        private int _currentLevel;
        private int _maxLevel;
        public int CurrentLevel => _currentLevel;
        public void Initialize(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _maxLevel = _staticDataService.UnitMaxLevel(_isHero);
        }
        public bool CanUpgrade(int gatesLevel) => 
            _currentLevel != _maxLevel && _currentLevel < gatesLevel;

        public void Upgrade(int levelToUpgrade, bool playFeedback)
        {
            if (_currentLevel == _maxLevel || _currentLevel == levelToUpgrade)
                return;
            
            _currentLevel = levelToUpgrade;
            ChangeEquipment();
            
            if(playFeedback)
                UpgradeFeedbacks.PlayFeedbacks();
        }

        protected virtual void ChangeEquipment()
        {
            EquipmentOnLevel equipmentOnLevel = _staticDataService.EquipmentForLevel(_currentLevel, _isHero);

            foreach (EquipmentItem item in equipmentOnLevel.EquipmentItems)
            {
                EquipmentMeshes.First(x => x.Category == item.Category).MeshRenderer.sharedMesh =
                    item.MeshRenderer.sharedMesh;
            }

            _weapon.sharedMesh = equipmentOnLevel.Weapon.sharedMesh;
            _shield.sharedMesh = equipmentOnLevel.Shield.sharedMesh;
        }
    }
}