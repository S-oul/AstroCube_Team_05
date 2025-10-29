using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RotateCubeInEditor))]
public class RotateCubeInEditorCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Texture2D whiteSquare =
            AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Assets/Game/Scripts/Tools/LD/RotationEditor/Editor/white_square.png");
        GUI.color = Color.white;

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(35);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            GUILayout.Space(35);
            
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Space(35);
                if (GUILayout.Button("←", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
                if (GUILayout.Button("←", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
                if (GUILayout.Button("←", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("↑", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                if (GUILayout.Button("↓", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(1);
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("↑", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                if (GUILayout.Button("↓", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(1);
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("↑", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                if (GUILayout.Button("↓", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Space(35);
                if (GUILayout.Button("→", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
                if (GUILayout.Button("→", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
                if (GUILayout.Button("→", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            }
            EditorGUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(35);
            if (GUILayout.Button("↖", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            if (GUILayout.Button("↖", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            if (GUILayout.Button("↖", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}));
            GUILayout.Space(35);
            
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(20);
        
        GUI.backgroundColor = Color.magenta;
        if (GUILayout.Button("Center Camera",
                new GUIStyle(GUI.skin.button)
                    { fixedHeight = 40, margin = new RectOffset(10, 10, 10, 10), fontSize = 20 }))
        {
            CenterCamera();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Completed Position", new GUIStyle(GUI.skin.label){fontSize = 20, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        {
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Save", new GUIStyle(GUI.skin.button){fontSize = 20, fontStyle = FontStyle.Bold}));
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Apply", new GUIStyle(GUI.skin.button){fontSize = 20, fontStyle = FontStyle.Bold}));
            
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        GUILayout.Label("Start Position", new GUIStyle(GUI.skin.label){fontSize = 20, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        {
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Save", new GUIStyle(GUI.skin.button){fontSize = 20, fontStyle = FontStyle.Bold}));
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Apply", new GUIStyle(GUI.skin.button){fontSize = 20, fontStyle = FontStyle.Bold}));
            
        }
        EditorGUILayout.EndHorizontal();
    }

    private void CenterCamera()
    {
        GameObject front = GameObject.Find("Main Rubik's Cube ");
        if (!front)
        {
            Debug.LogError("No cube detected in the scene.");
            return;
        }
        
        Selection.activeGameObject = front;
        
        SceneView sceneView = SceneView.lastActiveSceneView;
        sceneView.AlignViewToObject(front.transform);
        sceneView.size = 40f;
        sceneView.Repaint();
    }
}
