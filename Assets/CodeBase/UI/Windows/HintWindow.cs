using CodeBase.StaticData;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class HintWindow : BaseWindow
    {
        private const string Hide = "Hide";
        public Animator Animator;
        public TextMeshProUGUI HintText;
        private IStaticDataService _staticDataService;

        public void Initialize(string message)
        {
            HintText.text = message;
        }

        protected override void OnAwake()
        { 
            CloseButton.onClick.AddListener(HideWindow);
        }

        private void HideWindow()
        {
            CloseButton.gameObject.SetActive(false);
            Animator.SetTrigger(Hide);
            Destroy(gameObject, 0.7f);
        }
    }
}