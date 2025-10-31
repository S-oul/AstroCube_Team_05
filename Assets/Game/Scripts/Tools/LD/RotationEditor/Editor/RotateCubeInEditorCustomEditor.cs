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
        var cubeList = FindObjectsByType<RubicsCube>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
        if (cubeList.Length == 1)
        {
            GenerateCube(cubeList[0].gameObject);
        }
        
    }

    private void GenerateCube(GameObject newBaseCube)
    {
        foreach (RubicsCube cubes in FindObjectsByType<RubicsCube>(FindObjectsInactive.Include,
                     FindObjectsSortMode.InstanceID))
        {
            if (cubes.gameObject.name == "Temporary CopyCube")
            {
                DestroyImmediate(cubes.gameObject);
            }
            else
            {
                cubes.gameObject.SetActive(true);
            }
        }

        _baseCube = newBaseCube;
        _positionSaver = _baseCube.GetComponent<CubePositionSaver>();
        
        GameObject copyCube = Instantiate(_baseCube, _baseCube.transform.position, _baseCube.transform.rotation);
        DestroyImmediate(copyCube.GetComponent<CubePositionSaver>());
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
        
        if(_positionSaver.CompletedPositionSavedCount <= 0)
            SavePosition(false);

        CenterCamera();
    }

    private void OnDestroy()
    {
        ApplyPositionsOnRealCube();
        
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
        
        _baseCube = (GameObject) EditorGUILayout.ObjectField("Base Cube", _baseCube, typeof(GameObject), true);
        if (GUILayout.Button("Update Cube", new GUIStyle(GUI.skin.button) { fixedHeight = 40, margin = new RectOffset(10, 10, 10, 10), fontSize = 20 }))
        {
            GenerateCube(_baseCube);
        }
        EditorGUILayout.Space(20);

        if (!_baseCube || !GameObject.Find("Temporary CopyCube"))
            return;
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(35);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, true);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, true);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, true);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, false);
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, false);
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, false);
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
                ExecuteRotation(EditorCubeAxis.Depth, 1, false);
            if (GUILayout.Button("↖", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, false);
            if (GUILayout.Button("↖", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, false);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, true);
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, true);
            if (GUILayout.Button("↗", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, true);
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
        bool startPositionsExists = _positionSaver.StartPositionSavedCount > 0;
        EditorGUILayout.BeginHorizontal();
        {
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Save", new GUIStyle(GUI.skin.button){fontSize = 15, fixedHeight = 30}))
                SavePosition(true);
            GUI.backgroundColor = Color.green;
            
            GUI.enabled = startPositionsExists;
            if (GUILayout.Button("Apply", new GUIStyle(GUI.skin.button){fontSize = 15, fixedHeight = 30}))
                ApplyPosition(true);
            GUI.enabled = true;
        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.red;
        if(!startPositionsExists)
            GUILayout.Label("No positions detected");
        GUI.color = Color.white;
        
        GUILayout.Space(10);
        GUILayout.Label("Completed Position", new GUIStyle(GUI.skin.label){fontSize = 20, fontStyle = FontStyle.Bold});
        GUILayout.Label("Saved automatically at first start, can be overwritten", new GUIStyle(GUI.skin.label){fontSize = 10});
        GUILayout.Space(5);
        bool completedPositionsExists = _positionSaver.CompletedPositionSavedCount > 0;
        EditorGUILayout.BeginHorizontal();
        {
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Save", new GUIStyle(GUI.skin.button){fontSize = 15, fixedHeight = 30}))
                SavePosition(false);
            GUI.backgroundColor = Color.green;

            GUI.enabled = completedPositionsExists;
            if (GUILayout.Button("Apply", new GUIStyle(GUI.skin.button){fontSize = 15, fixedHeight = 30}))
                ApplyPosition(false);
            GUI.enabled = true;

        }
        EditorGUILayout.EndHorizontal();
        GUI.color = Color.red;
        if(!completedPositionsExists)
            GUILayout.Label("No positions detected");
        GUI.color = Color.white;
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

    private void ApplyPositionsOnRealCube()
    {
        GameObject copyCube = GameObject.Find("Temporary CopyCube");
        for (int i = 0; i < copyCube.transform.childCount; i++)
        {
            _copyToOriginalObjects[copyCube.transform.GetChild(i)].localPosition = copyCube.transform.GetChild(i).localPosition;
            _copyToOriginalObjects[copyCube.transform.GetChild(i)].localRotation = copyCube.transform.GetChild(i).localRotation;
            EditorUtility.SetDirty(_copyToOriginalObjects[copyCube.transform.GetChild(i)].gameObject);
        
            for (int j = 0; j < copyCube.transform.GetChild(i).childCount; j++)
            {
                _copyToOriginalObjects[copyCube.transform.GetChild(i).GetChild(j)].localPosition = copyCube.transform.GetChild(i).GetChild(j).localPosition;
                _copyToOriginalObjects[copyCube.transform.GetChild(i).GetChild(j)].localRotation = copyCube.transform.GetChild(i).GetChild(j).localRotation;
                EditorUtility.SetDirty(_copyToOriginalObjects[copyCube.transform.GetChild(i).GetChild(j)].gameObject);
            }
        }
    }
    
    private void SavePosition(bool isStartPosition)
    {
        ApplyPositionsOnRealCube();
        if (isStartPosition)
        {
            _positionSaver.SaveStartCubeState();
        }
        else
        {
            _positionSaver.SaveCompletedCubeState();
        }
    }

    private void ApplyPosition(bool isStartPosition)
    {
        Dictionary<GameObject, TransformState> positions;
        if (isStartPosition)
        {
            positions = _positionSaver.GetStartCubeState();
        }
        else
        {
            positions = _positionSaver.GetCompletedCubeState();
        }
    
        foreach(GameObject key in positions.Keys)
        {
            key.transform.localPosition = positions[key].localPosition;
            key.transform.localRotation = positions[key].localRotation;
        }

        foreach (Transform copyTransform in _copyToOriginalObjects.Keys)
        {
            copyTransform.localPosition = _copyToOriginalObjects[copyTransform].localPosition;
            copyTransform.localRotation = _copyToOriginalObjects[copyTransform].localRotation;
        }
    }
}

public enum EditorCubeAxis
{
    Horizontal = 0,
    Vertical = 1,
    Depth = 2
}
