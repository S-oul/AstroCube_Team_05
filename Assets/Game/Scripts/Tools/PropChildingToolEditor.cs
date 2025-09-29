using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropChildingTool))]
public class PropChildingToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Potassium");
    }
}
