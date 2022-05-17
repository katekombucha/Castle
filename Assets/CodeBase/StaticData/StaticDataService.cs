using System.Collections.Generic;
using System.Linq;
using CodeBase.Equipment;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataLevelsPath = "StaticData/Levels";
        private const string StaticDataWindowsPath = "StaticData/Windows/WindowStaticData";
        private const string StaticDataUnitEquipmentPath = "StaticData/Equipment/UnitEquipmentData";
        private const string StaticDataHeroEquipmentPath = "StaticData/Equipment/HeroEquipmentData";

        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<WindowId, WindowConfig> _windows;
        private Dictionary<int, EquipmentOnLevel> _unitEquipment;
        private Dictionary<int, EquipmentOnLevel> _heroEquipment;

        public Dictionary<string, LevelStaticData> Levels => _levels;
        public void LoadData()
        {
            _levels = Resources
                .LoadAll<LevelStaticData>(StaticDataLevelsPath)
                .ToDictionary(x => x.LevelKey, x => x);

            _windows = Resources
                .Load<WindowsStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);

            _unitEquipment = GetEquipmentData(StaticDataUnitEquipmentPath);
            _heroEquipment = GetEquipmentData(StaticDataHeroEquipmentPath);
        }

        private static Dictionary<int, EquipmentOnLevel> GetEquipmentData(string path)
        {
            return Resources
                .Load<EquipmentData>(path)
                .Equipments
                .ToDictionary(x => x.UnitLevel, x => x);
        }

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;

        public WindowConfig ForWindow(WindowId windowId) =>
            _windows.TryGetValue(windowId, out WindowConfig config)
                ? config
                : null;

        public EquipmentOnLevel EquipmentForLevel(int level, bool isHero)
        {
            Dictionary<int, EquipmentOnLevel> equipmentDictionary = isHero ? _heroEquipment : _unitEquipment;
            return equipmentDictionary.TryGetValue(level, out EquipmentOnLevel equipment)
                ? equipment
                : null;
        }

        public int UnitMaxLevel() => _unitEquipment.Count;
        public int UnitMaxLevel(bool isHero) => isHero ? _heroEquipment.Count : _unitEquipment.Count;

    }
}