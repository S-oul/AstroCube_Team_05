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

    private Dictionary<ECubeFace, List<Tile>> _tiles = new();
    private Dictionary<Tile, Vector3> _defaultPosition = new();

    
    [MenuItem("Tools/Cube/Spreader")]
    public static void Init()
    {
        CubeSpreadingEditorWindow window = GetWindowWithRect<CubeSpreadingEditorWindow>(new Rect(0, 0, 500, 500), true);
        window.Show();
    }

    private void CreateGUI()
    {
        foreach (ECubeFace face in Enum.GetValues(typeof(ECubeFace)))
        {
            _tiles[face] = new List<Tile>();
        }
        
        List<Tile> tiles = FindObjectsByType<Tile>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
            .Where(obj => obj.name == "Tile")
            .Where(obj => obj.GetComponentInParent<PreviewRubiksCube>() == null).ToList();

        foreach (Tile tile in tiles)
        {
            _defaultPosition[tile] = tile.transform.position;
            
            if (tile.transform.right == Vector3.up)
            {
                _tiles[ECubeFace.DOWN].Add(tile);
            } else if (tile.transform.right == Vector3.down)
            {
                _tiles[ECubeFace.UP].Add(tile);
            } else if (tile.transform.right == Vector3.left)
            {
                _tiles[ECubeFace.RIGHT].Add(tile);
            } else if (tile.transform.right == Vector3.right)
            {
                _tiles[ECubeFace.LEFT].Add(tile);
            } else if (tile.transform.right == Vector3.forward)
            {
                _tiles[ECubeFace.FRONT].Add(tile);
            } else
            {
                _tiles[ECubeFace.BACK].Add(tile);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (ECubeFace faces in Enum.GetValues(typeof(ECubeFace)))
        {
            foreach (Tile tile in _tiles[faces])
            {
                tile.transform.position = _defaultPosition[tile];
            }
        }
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
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("+", new GUIStyle(GUI.skin.button) { fixedHeight = 30, fixedWidth = 30, fontSize = 20 }))
            {
                _cubeSpreading += 5.0f;
            }
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("-", new GUIStyle(GUI.skin.button) { fixedHeight = 30, fixedWidth = 30, fontSize = 20 }))
            {
                _cubeSpreading -= 5.0f;
            }
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
        if (GUILayout.Button("Resume Spreading",
                new GUIStyle(GUI.skin.button)
                    { fixedHeight = 40, margin = new RectOffset(10, 10, 10, 10), fontSize = 20 }))
        {
            Close();
        }
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
        foreach (ECubeFace faces in Enum.GetValues(typeof(ECubeFace)))
        {
            if ((faces & _selectedCubeFaces) != 0)
            {
                foreach (Tile tile in _tiles[faces])
                {
                    tile.transform.position = _defaultPosition[tile] - tile.transform.right * _cubeSpreading;
                }
            }
            else
            {
                foreach (Tile tile in _tiles[faces])
                {
                    tile.transform.position = _defaultPosition[tile];
                }
            }
        }
    }
}
