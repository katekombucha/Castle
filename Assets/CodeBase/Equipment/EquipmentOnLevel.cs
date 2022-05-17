using System;
using UnityEngine;

namespace CodeBase.Equipment
{
    [Serializable]
    public class EquipmentOnLevel
    {
        public int UnitLevel;
        public EquipmentItem[] EquipmentItems;
        public MeshFilter Weapon;
        public MeshFilter Shield;
    }
}