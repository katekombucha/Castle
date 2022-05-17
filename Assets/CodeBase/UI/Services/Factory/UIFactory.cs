using CodeBase.GameResources;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.UI.Services.Factory
{
    class UIFactory : IUIFactory
    {
        private const string UIRootPath = "Prefabs/UI/UIRoot";
        private readonly IStaticDataService _staticData;
        private readonly IAssetProviderService _assets;
        private Transform _uiRoot;
        private IWindowService _windowService;
        private UIManager _uiManager;

        public UIFactory(IStaticDataService staticData, IAssetProviderService assets)
        {
            _staticData = staticData;
            _assets = assets;
        }

        public void CreateHud(ResourcesData resourcesData)
        {
            _windowService = AllServices.Container.Single<IWindowService>();
            _uiManager = _assets.Instantiate(AssetPath.HudPath).GetComponent<UIManager>();
            _uiManager.Construct(resourcesData);

            foreach (OpenWindowButton openWindowButton in _uiManager.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(_windowService);
        }

        public void CreateSettings()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Settings);
            Object.Instantiate(config.Prefab, _uiRoot);
        }

        public void CreateVictory()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Victory);
            Object.Instantiate(config.Prefab, _uiRoot);
        }

        public BaseWindow CreateTutorial(string sceneName, LevelStaticData data)
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Tutorial);
            BaseWindow tutorialWindow = Object.Instantiate(config.Prefab, _uiRoot);
            tutorialWindow.Construct(data);
            return tutorialWindow;
        }

        public void CreateMessage(string messageText)
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Hint);
            HintWindow hintWindow = (HintWindow) Object.Instantiate(config.Prefab, _uiRoot);
            hintWindow.Initialize(messageText);
        }

        public Pointer CreatePointer()
        {
            Pointer pointer = _assets.Instantiate(AssetPath.PointerPath,
                _uiManager.PointersPanel).GetComponent<Pointer>();

            return pointer;
        }

        public void CreateUIRoot() => 
            _uiRoot = _assets.Instantiate(UIRootPath).transform;
    }
}