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

    private bool _front, _right, _up, _down, _left, _back;
    private float _cubeSpreading;
    private ECubeFace _selectedCubeFaces;

    private Dictionary<ECubeFace, GameObject> _faces = new();

    
    [MenuItem("Tools/Cube/Spreader")]
    public static void Init()
    {
        CubeSpreadingEditorWindow window = GetWindowWithRect<CubeSpreadingEditorWindow>(new Rect(0, 0, 500, 500), true);
        window.Show();
    }

    private void CreateGUI()
    {
        GameObject cubeLogic = GameObject.Find("CubeLogic");
        _faces[ECubeFace.FRONT] = cubeLogic.transform.Find("ZBack").gameObject;
        _faces[ECubeFace.UP] = cubeLogic.transform.Find("YUp").gameObject;
        _faces[ECubeFace.DOWN] = cubeLogic.transform.Find("YDown").gameObject;
        _faces[ECubeFace.LEFT] = cubeLogic.transform.Find("XBack").gameObject;
        _faces[ECubeFace.RIGHT] = cubeLogic.transform.Find("XFront").gameObject;
        _faces[ECubeFace.BACK] = cubeLogic.transform.Find("ZFront").gameObject;
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
                _front = EditorGUILayout.Toggle("Front", _front, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                _up = EditorGUILayout.Toggle("Up", _up, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                _right = EditorGUILayout.Toggle("Right", _right, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                _down = EditorGUILayout.Toggle("Down", _down, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                _left = EditorGUILayout.Toggle("Left", _left, new GUIStyle(GUI.skin.toggle));
                GUILayout.FlexibleSpace();
                _back = EditorGUILayout.Toggle("Back", _back, new GUIStyle(GUI.skin.toggle));
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
            _cubeSpreading = EditorGUILayout.FloatField(_cubeSpreading, new GUIStyle(GUI.skin.textField){margin = new RectOffset(25, 50, 0, 0), fontSize = 18, fontStyle = FontStyle.Bold, fixedHeight = 30, alignment = TextAnchor.MiddleCenter});
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = Color.magenta;
        if (GUILayout.Button("Center Camera",
                new GUIStyle(GUI.skin.button)
                    { fixedHeight = 40, margin = new RectOffset(10, 10, 10, 10), fontSize = 20 }))
        {
            CenterCamera();
        }
        GUI.backgroundColor = Color.red;
        GUILayout.Button("Resume Spreading", new GUIStyle(GUI.skin.button){fixedHeight = 40, margin = new RectOffset(10, 10, 10, 10), fontSize = 20});
        GUI.backgroundColor = Color.white;
        
        UpdateFaces();
        ApplyCubeSpreading();
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
        sceneView.size = 50f;
        sceneView.Repaint();
    }

    private void UpdateFaces()
    {
        _selectedCubeFaces = 0;
        if (_front) _selectedCubeFaces |= ECubeFace.FRONT;
        if (_right) _selectedCubeFaces |= ECubeFace.RIGHT;
        if (_up) _selectedCubeFaces |= ECubeFace.UP;
        if (_down) _selectedCubeFaces |= ECubeFace.DOWN;
        if (_left) _selectedCubeFaces |= ECubeFace.LEFT;
        if (_back) _selectedCubeFaces |= ECubeFace.BACK;
    }

    private void ApplyCubeSpreading()
    {
        foreach (ECubeFace face in Enum.GetValues(typeof(ECubeFace)))
        {
            if ((_selectedCubeFaces & face) != 0)
            {
                GameObject faceObj = _faces[face];
                
            }
        }
    }
}
