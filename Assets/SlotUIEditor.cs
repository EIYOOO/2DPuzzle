#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlotUI))]
public class SlotUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SlotUI slotUI = (SlotUI)target;
        if (GUILayout.Button("Capture Position"))
        {
            RectTransform rt = slotUI.itemImage.rectTransform;
            Debug.Log($"当前坐标: {rt.anchoredPosition}");
            Debug.Log($"当前RaycastPadding: {slotUI.itemImage.raycastPadding}");
        }
    }
}
#endif
