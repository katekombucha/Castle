using CodeBase.Infrastructure.Services;
using CodeBase.Services.Formation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class ArmyUpgradeUI : MonoBehaviour
    {
        public Image [] ArrowImages;
        public Image FireImage;
        public Color BlueColor;
        private IFormationService _formationService;
        private int _currentArrowsCount;

        private void Start()
        {
            _formationService = AllServices.Container.Single<IFormationService>();
            _formationService.OnArmyLevelChange += UpdateArrowsCount;
            _currentArrowsCount = 0;
        }

        private void UpdateArrowsCount(float armyLevel)
        {
            if (armyLevel == 0)
            {
                foreach (Image arrowImage in ArrowImages)
                {
                    arrowImage.color = new Color(BlueColor.r, BlueColor.g, BlueColor.b, 0);
                }
                return;
            }

            if (_currentArrowsCount < armyLevel )
            {
                ArrowImages[_currentArrowsCount].color = BlueColor;
            }

            _currentArrowsCount = (int) armyLevel;
            ArrowImages[_currentArrowsCount].color = new Color(BlueColor.r, BlueColor.g, BlueColor.b, armyLevel-_currentArrowsCount);
        }

        public void ShowFire() => 
            FireImage.DOFade(1, 0.3f);

        public void HideFire() => 
            FireImage.DOFade(0, 0.1f);

        private void OnDisable() => 
            _formationService.OnArmyLevelChange -= UpdateArrowsCount;
    }
}
