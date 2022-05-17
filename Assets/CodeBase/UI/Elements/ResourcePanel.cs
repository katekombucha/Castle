using CodeBase.GameResources;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ResourcePanel : MonoBehaviour
    {
        public RectTransform RectTransform;
        public ResourceType ResourceType;
        public TextMeshProUGUI Text;
        private ResourcesData _resourcesData;
        private readonly Vector2 _feedbackPosition = new Vector2(15, 0);

        public void Construct(ResourcesData resourcesData)
        {
            _resourcesData = resourcesData;
            UpdateResource();
        }

        public void UpdateResource()
        {
            int count = _resourcesData.Collected(ResourceType);
            Text.text = count.ToString();
            
            RectTransform.DOComplete();
            RectTransform.DOPunchAnchorPos(_feedbackPosition, 0.2f, 1);
        }
    }
}