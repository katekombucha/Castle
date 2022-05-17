using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CodeBase.GameResources
{
    public class ResourceStack : MonoBehaviour, ISavedProgress
    {
        public Transform ResourcePlace;
        public MMFeedbacks CollectFeedbacks;
        public MMFeedbacks SpentFeedbacks;
        public Action<int> OnResourcesChanged;

        private List<ResourceInStack> _resources;
        private ResourcesData _resourcesData;
        private IGameFactory _gameFactory;
        private float _movementDuration = 0.3f;
        private float _jumpPower;

        public void Construct(ResourcesData resourcesData, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _resourcesData = resourcesData;
            _resources = new List<ResourceInStack>();
        }

        public bool HaveResources(ResourceType resourceType, int requiredAmount = 0) => 
            _resourcesData.HaveResources(resourceType, requiredAmount);

        public void ChangeJumpSettings()
        {
            _movementDuration = 2f;
            _jumpPower = 7;
        }

        public void AddResource(Transform resourceTransform, ResourceType resourceType, bool feedback = false)
        {
            if (feedback)
                CollectFeedbacks.PlayFeedbacks();
            
            Vector3 pos = new Vector3(0,0.125f * _resources.Count, 0);
            resourceTransform.SetParent(ResourcePlace);
            resourceTransform.DOLocalJump(pos, _jumpPower, 1, _movementDuration).SetEase(Ease.OutBack);

            _resources.Add(new ResourceInStack(resourceType, resourceTransform));
            _resourcesData.Collect(resourceType);
            
            OnResourcesChanged?.Invoke(_resources.Count);
        }

        public void SpendResource(Vector3 at, ResourceType resourceType)
        {
            if(!HaveResources(resourceType))
                return;
        
            _resourcesData.Spend(resourceType);
        
            ResourceInStack res = _resources.Last(x => x.ResourceType == resourceType);
            _resources.Remove(res);

            Transform resource = res.Transform;
            resource.transform.parent = null;
        
            resource.transform.DOJump(at, 1, 1,_movementDuration)
                .OnComplete(() =>
                {
                    SpentFeedbacks.PlayFeedbacks();
                    Destroy(resource.gameObject, 0.1f);
                });
            
            OnResourcesChanged?.Invoke(_resources.Count);
        }

        public void LoadProgress(PlayerProgress progress, string sceneName)
        {
            int coinsCount = progress.CoinsAmount;

            for (int i = 0; i < coinsCount; i++)
            {
                Vector3 pos = new Vector3(0,0.125f * i, 0);
                Collectible coin = _gameFactory.CreateCoin(pos);
                coin.CreateInStack(this);
            }
        }

        public void UpdateProgress(PlayerProgress progress, string levelName)
        {
            progress.CoinsAmount = _resources.Count;
        }
        
        private class ResourceInStack
        {
            public readonly ResourceType ResourceType;
            public readonly Transform Transform;

            public ResourceInStack(ResourceType resourceType, Transform transform)
            {
                ResourceType = resourceType;
                Transform = transform;
            }
        }
    }
}
