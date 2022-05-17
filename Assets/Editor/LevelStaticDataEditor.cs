using System.Linq;
using CodeBase.GameEntities;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelStaticData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                EntityMarker[] markers = FindObjectsOfType<EntityMarker>();

                levelStaticData.Villages = markers
                    .Where(x => x.EntityType == EntityType.Village)
                    .Select(x => new VillageData(x.transform.position, x.Id)).ToArray();

                EntityMarker dragonMarker = markers.FirstOrDefault(x => x.EntityType == EntityType.Dragon);
                levelStaticData.DragonInfo.DragonPosition = dragonMarker != null ? dragonMarker.transform.position : Vector3.zero;
                
                EntityMarker dragonBlockMarker = markers.FirstOrDefault(x => x.EntityType == EntityType.DragonBlock);
                levelStaticData.DragonBlockPosition = dragonBlockMarker != null ? dragonBlockMarker.transform.position : Vector3.zero;
                
                levelStaticData.GatesPosition = markers.FirstOrDefault(x => x.EntityType == EntityType.Gates).transform.position;
            }

            EditorUtility.SetDirty(target);
        }
    }
}