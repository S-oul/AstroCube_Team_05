using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropChildingTool))]
public class PropChildingToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PropChildingTool propChildingTool = (PropChildingTool)target;
        if (GUILayout.Button("Run Tool"))
        {
            propChildingTool.RunTool();
        }
    }
}
