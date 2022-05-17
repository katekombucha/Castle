using System;
using System.Collections.Generic;
using CodeBase.StaticData;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public int GatesLevel;
        public int GatesUpgradeShortage;
        public int CastleCitizens;
        public int ArmyCount;
        public int[] UnitsLevels;
        public Dictionary<int, VillageData> VillageData;
        public bool TutorialComplete;

        public WorldData(){}
        public WorldData(LevelStaticData levelData)
        {
            CastleCitizens = levelData.StartingNumberOfCitizens;
            GatesLevel = 0;
            GatesUpgradeShortage = 0;
            ArmyCount = 0;
            UnitsLevels = new int[0];
            VillageData = new Dictionary<int, VillageData>();
            
            for (int i = 0; i < levelData.Villages.Length; i++)
            {
                VillageData.Add(i, new VillageData(i));
            }
        }
    }
}