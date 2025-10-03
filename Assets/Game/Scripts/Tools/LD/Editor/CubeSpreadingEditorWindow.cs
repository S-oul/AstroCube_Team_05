using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DG.Tweening;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEngine;

[EditorWindowTitle(title = "Cube Spread", icon = "Assets/Game/Scripts/Tools/LD/Editor/CubeSpreader/rubik.png")]
public class CubeSpreadingEditorWindow : EditorWindow
{

    [MenuItem("Tools/Cube/Spreader")]
    public static void Init()
    {
        CubeSpreadingEditorWindow window = GetWindowWithRect<CubeSpreadingEditorWindow>(new Rect(0, 0, 500, 500), true);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(20);
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Game/Scripts/Tools/LD/Editor/CubeSpreader/rubik.png"), new GUIStyle(GUI.skin.label){fixedHeight = 64, fixedWidth = 64});
            GUILayout.Label("Cube Spreader", new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter, fontSize = 30, fontStyle = FontStyle.Bold, fixedHeight = 64});
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(new GUIStyle{fixedHeight = 250});
            {
                GUILayout.Space(40);
                EditorGUILayout.Toggle("Front", false, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                EditorGUILayout.Toggle("Up", false, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                EditorGUILayout.Toggle("Right", false, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                EditorGUILayout.Toggle("Down", false, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                EditorGUILayout.Toggle("Left", false, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                EditorGUILayout.Toggle("Back", false, new GUIStyle(GUI.skin.toggle));
                GUILayout.Space(40);
            }
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("Spread Distance", new GUIStyle(GUI.skin.label){fontSize = 18, fontStyle = FontStyle.Bold, fixedHeight = 30});
            GUILayout.FlexibleSpace();
            EditorGUILayout.FloatField(0.0f, new GUIStyle(GUI.skin.textField){margin = new RectOffset(25, 50, 0, 0), fontSize = 18, fontStyle = FontStyle.Bold, fixedHeight = 30, alignment = TextAnchor.MiddleCenter});
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = Color.magenta;
        GUILayout.Button("Center Camera", new GUIStyle(GUI.skin.button){fixedHeight = 40, margin = new RectOffset(10, 10, 10, 10), fontSize = 20});
        GUI.backgroundColor = Color.red;
        GUILayout.Button("Resume Spreading", new GUIStyle(GUI.skin.button){fixedHeight = 40, margin = new RectOffset(10, 10, 10, 10), fontSize = 20});
        GUI.backgroundColor = Color.white;
    }
}
