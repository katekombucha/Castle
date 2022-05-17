using System;
using System.Collections.Generic;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public string CurrentLevel;
        public int CoinsAmount;
        public Dictionary<string, WorldData> DataOnLevel;
        public bool GatesHintsShown;

        public PlayerProgress(){}
        public PlayerProgress(string currentLevelName, IStaticDataService staticDataService)
        {
            CurrentLevel = currentLevelName;
            CoinsAmount = 0;
            DataOnLevel = new Dictionary<string, WorldData>();
            GatesHintsShown = false;

            for (int i = 1; i <= staticDataService.Levels.Count; i++)
            {
                string name = "Level " + i;
                DataOnLevel.Add(name, new WorldData(staticDataService.Levels[name]));
            }
        }
    }
}