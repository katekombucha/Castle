using System;
using UnityEngine;

namespace CodeBase.Equipment
{
    [Serializable]
    public class EquipmentItem
    {
        public SkinnedMeshRenderer MeshRenderer;
        public EquipmentItemCategory Category;

        public EquipmentItem(SkinnedMeshRenderer mesh, EquipmentItemCategory category)
        {
            MeshRenderer = mesh;
            Category = category;
        }
    }

    public enum EquipmentItemCategory
    {
        Armor = 0,
        Body = 1,
        Hands = 2,
        Head = 3,
        Foot = 4,
        Shield = 5,
        Weapon = 6,
    }
}