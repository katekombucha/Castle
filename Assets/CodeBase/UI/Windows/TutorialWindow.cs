using CodeBase.StaticData;
using TMPro;

namespace CodeBase.UI.Windows
{
    public class TutorialWindow : BaseWindow
    {
        public TextMeshProUGUI TutorialText;
        private IStaticDataService _staticDataService;

        protected override void Initialize()
        {
            TutorialText.text = _data.TutorialMessages[0];
        }
    }
}