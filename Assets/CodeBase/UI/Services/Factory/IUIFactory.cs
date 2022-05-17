using CodeBase.GameResources;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Windows;

namespace CodeBase.UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateSettings();
        void CreateUIRoot();
        Pointer CreatePointer();
        void CreateHud(ResourcesData resourcesData);
        void CreateVictory();
        BaseWindow CreateTutorial(string sceneName, LevelStaticData data);
        void CreateMessage(string messageText);
    }
}