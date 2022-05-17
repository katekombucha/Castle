using System.Collections;
using CodeBase.GameEntities;
using CodeBase.UI.Elements;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.GameResources
{
    public class ResourceConsumer : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public CostUIController CostUIController;
        public MMFeedbacks UpgradeFeedback;
        [SerializeField] protected ResourceType _requiredResource;
        private IUpgradablePlace _upgradablePlace;
        private int _cost = 10;
        private int _resourcesShortage = 10;
        private bool _collideWithPlayer;
        private float _delay = 0.1f;
        private Coroutine _getResources;

        private void Start()
        {
            _upgradablePlace = GetComponentInParent<IUpgradablePlace>();

            if (_upgradablePlace.CheckIfMaxLevel())
            {
                DisableUpgrade();
                return;
            }
        
            _cost = _upgradablePlace.GetUpgradeCost();
            _resourcesShortage = _upgradablePlace.ResourceShortage;
            CostUIController.ChangeResourcesCount(resourcesToConsume: _cost, resourcesShortage: _resourcesShortage);

            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;
        }

        private void TriggerEnter(Collider obj)
        {
            _collideWithPlayer = true;
            GetResources(obj.GetComponent<ResourceStack>());
        }

        private void TriggerExit(Collider obj)
        {
            _collideWithPlayer = false;
        }

        private void GetResources(ResourceStack resourceStack)
        {
            _getResources = StartCoroutine(GetResourcesWithDelay(resourceStack));
        }

        private IEnumerator GetResourcesWithDelay(ResourceStack resourceStack)
        {
            yield return new WaitForSeconds(0.2f);
            
            while (_collideWithPlayer &&  
                   resourceStack.HaveResources(_requiredResource) && 
                   !CheckIfEnoughResources())
            {
                resourceStack.SpendResource(transform.position, _requiredResource);

                UpdateCostInfo();

                if (CheckIfEnoughResources())
                {
                    GetEnoughResources();
                }
                yield return new WaitForSeconds(_delay);
            }
        }

        private void GetEnoughResources()
        {
            _collideWithPlayer = false;
            _upgradablePlace.Upgrade(1);
            UpgradeFeedback.PlayFeedbacks();
            SetNewCost();
        }
    
        private void SetNewCost()
        {
            if (_upgradablePlace.CheckIfMaxLevel())
                DisableUpgrade();

            _cost = _upgradablePlace.GetUpgradeCost();
            _resourcesShortage = _cost;
            CostUIController.ChangeResourcesCount(_cost,  _resourcesShortage);
        }

        private bool CheckIfEnoughResources()
        {
            return _resourcesShortage == 0;
        }

        private void UpdateCostInfo()
        {
            _resourcesShortage--;
            _upgradablePlace.ResourceShortage = _resourcesShortage;
            CostUIController.ChangeResourcesCount(resourcesToConsume: _cost, resourcesShortage: _resourcesShortage);
        }

        private void DisableUpgrade()
        {
            if (_getResources != null) 
                StopCoroutine(_getResources);
        
            Unsubscribe();
            CostUIController.DisableUI();
        }

        private void Unsubscribe()
        {
            TriggerObserver.TriggerEnter -= TriggerEnter;
            TriggerObserver.TriggerExit -= TriggerExit;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}