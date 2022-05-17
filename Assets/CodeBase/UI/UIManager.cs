using CodeBase.GameResources;
using CodeBase.UI.Elements;
using UnityEngine;

namespace CodeBase.UI
{
    public class UIManager : MonoBehaviour
    {
        public ResourcePanel[] ResourcePanels;
        public Transform PointersPanel;
        private ResourcesData _resourcesData;

        public void Construct(ResourcesData resourcesData) => 
            _resourcesData = resourcesData;

        private void Start()
        {
            foreach (ResourcePanel resourcePanel in ResourcePanels)
            {
                resourcePanel.Construct(_resourcesData);
            }
            SetEvents();
        }

        private void SetEvents() => 
            _resourcesData.OnChanged += UpdateResource;

        private void UpdateResource(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Coin:
                    ResourcePanels[0].UpdateResource();
                    break;
            }
        }

        private void OnDisable() => 
            _resourcesData.OnChanged -= UpdateResource;
    }
}
