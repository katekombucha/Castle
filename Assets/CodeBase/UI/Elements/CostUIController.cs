using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class CostUIController : MonoBehaviour
    {
        public TextMeshProUGUI ResourcesCountText;
        public Image FillImage;
        public RectTransform ResourcesPanel;
        public GameObject ResourceCanvas;

        public void ChangeResourcesCount(int resourcesToConsume, int resourcesShortage)
        {
            ResourcesCountText.text = resourcesShortage.ToString();
            ResourcesPanel.DOPunchScale(new Vector2(0.2f, 0.2f), 0.1f, 1, 1);
            FillImage.fillAmount =  1 - (float)resourcesShortage / resourcesToConsume;
        }

        public void DisableUI() => 
            ResourceCanvas.SetActive(false);
    }
}