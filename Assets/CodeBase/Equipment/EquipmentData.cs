using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Equipment
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Equipment")]
    public class EquipmentData : ScriptableObject
    {
        public List<EquipmentOnLevel> Equipments;
    }
}