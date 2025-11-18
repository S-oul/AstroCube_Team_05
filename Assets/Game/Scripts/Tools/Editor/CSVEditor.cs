using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DG.Tweening;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEngine;

[EditorWindowTitle(title = "CSV Editor", icon = "Assets/Game/Scripts/Tools/LD/Editor/CubeSpreader/rubik.png")]
public class CSVEditor : EditorWindow
{
    
    [MenuItem("Tools/Localization")]
    public static void Init()
    {
        CSVEditor window = GetWindowWithRect<CSVEditor>(new Rect(0, 0, 900, 600), true);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(20);
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Game/Scripts/Tools/LD/Editor/CubeSpreader/rubik.png"), new GUIStyle(GUI.skin.label){fixedHeight = 64, fixedWidth = 64});
            GUILayout.Label("CSV Editor", new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter, fontSize = 30, fontStyle = FontStyle.Bold, fixedHeight = 64});
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }
}
