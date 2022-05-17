using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            Debug.Log(CloseButton.name);
        }
    }
}