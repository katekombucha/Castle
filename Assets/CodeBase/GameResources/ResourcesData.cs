using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeBase.GameResources
{
    [Serializable]
    public class ResourcesData
    {
        public Action <ResourceType> OnChanged;
        private List<ResourceAmount> _resourceAmounts;
        private int _resourceTypeOrder = -1;

        public ResourcesData()
        {
            _resourceAmounts = new List<ResourceAmount>()
            {
                new ResourceAmount(ResourceType.Coin, 0),
            };
        }
        public void Collect(ResourceType resourceType)
        {
            Resource(resourceType).Amount += 1;
            OnChanged?.Invoke(resourceType);
        }

        public int Collected(ResourceType resourceType) => 
            Resource(resourceType).Amount;

        public bool HaveResources(ResourceType resourceType, int requiredAmount) => 
            Resource(resourceType).Amount > requiredAmount;

        public int ResourcePlaceInList()
        {
            _resourceTypeOrder++;
            return _resourceTypeOrder;
        }
        public int AmountForExchange(ResourceType resourceType, int exchangeRate)
        {
            int amount = Resource(resourceType).Amount;
            return amount - (amount % exchangeRate);
        }
        private ResourceAmount Resource(ResourceType resourceType)
        {
            return _resourceAmounts.First(x => x.ResourceType == resourceType);
        }

        public void Spend(ResourceType resourceType)
        {
            Resource(resourceType).Amount -= 1;
            OnChanged?.Invoke(resourceType);
        }

        private class ResourceAmount
        {
            public ResourceType ResourceType;
            public int Amount;

            public ResourceAmount(ResourceType resourceType, int amount)
            {
                ResourceType = resourceType;
                Amount = amount;
            }
        }
    }
}