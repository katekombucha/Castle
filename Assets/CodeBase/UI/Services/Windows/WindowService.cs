using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Open(WindowId windowId)
        {
            Debug.Log("Open " + windowId);
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.Settings:
                    _uiFactory.CreateSettings();
                    break;
            }
        }
    }
}