using System;
using UnityEngine;

namespace CodeBase.StaticData
{
    [Serializable]
    public class VillageData
    {
        public Vector3 Position;
        public int Id;
        public int Level;
        public int ResourceShortage;

        public VillageData() { }
        public VillageData(int id)
        {
            Id = id;
        }
        public VillageData(Vector3 position, int id)
        {
            Position = position;
            Id = id;
        }

        public void SaveUpgradeData(int level, int resourceShortage)
        {
            Level = level;
            ResourceShortage = resourceShortage;
        }
    }
}