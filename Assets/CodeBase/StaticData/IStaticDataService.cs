using System.Collections.Generic;
using CodeBase.Equipment;
using CodeBase.Infrastructure.Services;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.StaticData
{
    public interface IStaticDataService : IService
    {
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId windowId);
        void LoadData();
        EquipmentOnLevel EquipmentForLevel(int level, bool isHero);
        int UnitMaxLevel();
        int UnitMaxLevel(bool isHero);
        Dictionary<string, LevelStaticData> Levels { get; }
    }
}