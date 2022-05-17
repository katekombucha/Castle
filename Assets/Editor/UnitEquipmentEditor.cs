using CodeBase.Equipment;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitEquipment))]
public class UnitEquipmentEditor : UnityEditor.Editor
{
    public int gatesLevel;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UnitEquipment unitEquipment = (UnitEquipment) target;

        if (GUILayout.Button("Upgrade"))
        {
            if (Application.isPlaying)
            {
                unitEquipment.Upgrade(gatesLevel, false);
            }
        }

        if (GUILayout.Button("Collect SkinnedMeshRenderers"))
        {
            SkinnedMeshRenderer[] skins = unitEquipment.GetComponentsInChildren<SkinnedMeshRenderer>();
            unitEquipment.EquipmentMeshes = new EquipmentItem [5];
            for (int i = 0; i < 5; i++)
            {
                unitEquipment.EquipmentMeshes[i] = new EquipmentItem(skins[i], (EquipmentItemCategory) i);
            }
        }

        EditorUtility.SetDirty(target);
    }
}