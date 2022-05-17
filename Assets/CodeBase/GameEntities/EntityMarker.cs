using UnityEngine;

namespace CodeBase.GameEntities
{
    public class EntityMarker : MonoBehaviour
    {
        public EntityType EntityType;
        public int Id;
    }
    
    public enum EntityType
    {
        Gates,
        Dragon,
        Village,
        DragonBlock
    }
}