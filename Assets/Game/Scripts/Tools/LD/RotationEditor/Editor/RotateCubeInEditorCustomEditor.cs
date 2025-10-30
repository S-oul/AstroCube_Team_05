using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[EditorWindowTitle(title = "Cube Modifier", icon = "Assets/Game/Scripts/Tools/LD/Editor/CubeSpreader/rubik.png")]
public class RotateCubeInEditorEditorWindow : EditorWindow
{

    private GameObject _baseCube;
    private RubiksMovement _rubiksMovement;
    private CubePositionSaver _positionSaver;

    private Dictionary<Transform, Transform> _copyToOriginalObjects = new();
    
    [MenuItem("Tools/Cube/Modifier")]
    public static void Init()
    {
        RotateCubeInEditorEditorWindow window = GetWindowWithRect<RotateCubeInEditorEditorWindow>(new Rect(0, 0, 500, 700), true);
        window.Show();
    }

    private void CreateGUI()
    {
        _baseCube = GameObject.Find("Main Rubik's Cube ");
        GameObject copyCube = Instantiate(_baseCube, _baseCube.transform.position, _baseCube.transform.rotation);
        copyCube.name = "Temporary CopyCube";
        
        _rubiksMovement = copyCube.GetComponentInChildren<RubiksMovement>();
        _baseCube.SetActive(false);

        for (int i = 0; i < copyCube.transform.childCount; i++)
        {
            _copyToOriginalObjects[copyCube.transform.GetChild(i)] =
                _baseCube.transform.GetChild(i);
            for (int j = 0; j < copyCube.transform.GetChild(i).childCount; j++)
            {
                _copyToOriginalObjects[copyCube.transform.GetChild(i).GetChild(j)] =
                    _baseCube.transform.GetChild(i).GetChild(j);
            }
        }

        CenterCamera();
    }

    private void OnDestroy()
    {
        GameObject copyCube = GameObject.Find("Temporary CopyCube");
        for (int i = 0; i < copyCube.transform.childCount; i++)
        {
            _copyToOriginalObjects[copyCube.transform.GetChild(i)].localPosition = copyCube.transform.GetChild(i).localPosition;
            _copyToOriginalObjects[copyCube.transform.GetChild(i)].localRotation = copyCube.transform.GetChild(i).localRotation;
            for (int j = 0; j < copyCube.transform.GetChild(i).childCount; j++)
            {
                _copyToOriginalObjects[copyCube.transform.GetChild(i).GetChild(j)].localPosition = copyCube.transform.GetChild(i).GetChild(j).localPosition;
                _copyToOriginalObjects[copyCube.transform.GetChild(i).GetChild(j)].localRotation = copyCube.transform.GetChild(i).GetChild(j).localRotation;
            }
        }
        
        DestroyImmediate(GameObject.Find("Temporary CopyCube"));
        
        _baseCube.SetActive(true);
        Selection.activeGameObject = _baseCube;
    }


    private void OnGUI()
    {
        Texture2D whiteSquare =
            AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Assets/Game/Scripts/Tools/LD/RotationEditor/Editor/white_square.png");

        GUI.backgroundColor = Color.white;
        GUI.color = Color.white;
        
        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Game/Scripts/Tools/LD/Editor/CubeSpreader/rubik.png"), new GUIStyle(GUI.skin.label){fixedHeight = 64, fixedWidth = 64});
            GUILayout.Label("Cube Modifier", new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter, fontSize = 30, fontStyle = FontStyle.Bold, fixedHeight = 64});
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(35);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, true);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, true);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, true);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, false);
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, false);
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, false);
            GUILayout.Space(35);
            
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Space(35);
                if (GUILayout.Button("←", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Vertical, 3, true);
                if (GUILayout.Button("←", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Vertical, 2, true);
                if (GUILayout.Button("←", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Vertical, 1, true);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("↑", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Horizontal, 1, true);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                if (GUILayout.Button("↓", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Horizontal, 1, false);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(1);
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("↑", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Horizontal, 2, true);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                if (GUILayout.Button("↓", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Horizontal, 2, false);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(1);
            EditorGUILayout.BeginVertical();
            {
                if (GUILayout.Button("↑", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Horizontal, 3, true);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                GUILayout.Space(1);
                GUILayout.Label(whiteSquare, new GUIStyle(GUI.skin.label){fixedWidth = 32, fixedHeight = 32});
                if (GUILayout.Button("↓", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Horizontal, 3, false);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Space(35);
                if (GUILayout.Button("→", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Vertical, 1, false);
                if (GUILayout.Button("→", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Vertical, 2, false);
                if (GUILayout.Button("→", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                    ExecuteRotation(EditorCubeAxis.Vertical, 3, false);
            }
            EditorGUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(35);
            if (GUILayout.Button("↖", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, false);
            if (GUILayout.Button("↖", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, false);
            if (GUILayout.Button("↖", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, false);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, true);
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, true);
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, true);
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
        GUILayout.Label("Start Position", new GUIStyle(GUI.skin.label){fontSize = 20, fontStyle = FontStyle.Bold});
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        {
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Save", new GUIStyle(GUI.skin.button){fontSize = 15, fixedHeight = 30}));
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Apply", new GUIStyle(GUI.skin.button){fontSize = 15, fixedHeight = 30}));
            
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        GUILayout.Label("Completed Position", new GUIStyle(GUI.skin.label){fontSize = 20, fontStyle = FontStyle.Bold});
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        {
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Save", new GUIStyle(GUI.skin.button){fontSize = 15, fixedHeight = 30}));
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Apply", new GUIStyle(GUI.skin.button){fontSize = 15, fixedHeight = 30}));
            
        }
        EditorGUILayout.EndHorizontal();
    }

    private void CenterCamera()
    {
        GameObject front = GameObject.Find("Temporary CopyCube");
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

    ///<summary>
    /// Fait une rotation In Editor du cube
    ///</summary>
    /// <param name="axis">Horizontal pour |||, Vertical pour ≡ et Depth pour les faces en profondeur</param> 
    /// <param name="row">La ligne de l'axe qui sera concernée par la rotation</param>
    /// <param name="clockwise">Si la rotation est dans le sens des aiguilles d'une poutre</param>
    private void ExecuteRotation(EditorCubeAxis axis, int row, bool clockwise)
    {
        switch (axis)
        {
            case EditorCubeAxis.Horizontal:
                switch (row)
                {
                    case 1:
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[4], _rubiksMovement.LeftCenterCube, clockwise);
                        break;
                    case 2:
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[4], _rubiksMovement.MiddleCenterCube, clockwise);
                        break;
                    case 3: 
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[4], _rubiksMovement.RightCenterCube, clockwise);
                        break;
                }
                break;
            case EditorCubeAxis.Vertical:
                switch (row)
                {
                    case 1:
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[2], _rubiksMovement.BottomCenterCube, clockwise);
                        break;
                    case 2:
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[2], _rubiksMovement.MiddleCenterCube, clockwise);
                        break;
                    case 3: 
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[2], _rubiksMovement.TopCenterCube, clockwise);
                        break;
                }
                break;
            case EditorCubeAxis.Depth:
                switch (row)
                {
                    case 1:
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[0], _rubiksMovement.FrontCenterCube, clockwise);
                        break;
                    case 2:
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[0], _rubiksMovement.MiddleCenterCube, clockwise);
                        break;
                    case 3: 
                        _rubiksMovement.RotateInEditor(_rubiksMovement.Axis[0], _rubiksMovement.BackCenterCube, clockwise);
                        break;
                }
                break;
        }
    }
}

public enum EditorCubeAxis
{
    Horizontal = 0,
    Vertical = 1,
    Depth = 2
}
