using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RotateCubeInEditor))]
public class RotateCubeInEditorCustomEditor : Editor
{
    private RotateCubeInEditor cubeData => (RotateCubeInEditor)target;
    private RubiksMovement rubiksMovement => cubeData.rubiksMovement;
    
    public override void OnInspectorGUI()
    {
        Texture2D whiteSquare =
            AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Assets/Game/Scripts/Tools/LD/RotationEditor/Editor/white_square.png");
        GUI.color = Color.white;

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(35);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, false);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, false);
            if (GUILayout.Button("↙", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, false);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 1, true);
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 2, true);
            if (GUILayout.Button("↘", new GUIStyle(GUI.skin.button){fixedWidth = 32, fixedHeight = 32}))
                ExecuteRotation(EditorCubeAxis.Depth, 3, true);
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

    ///<summary>
    /// Fait une rotation In Editor du cube
    ///</summary>
    /// <param name="axis">Horizontal pour |||, Vertical pour ≡ et Depth pour les faces en profondeur</param> 
    /// <param name="row">La ligne de l'axe qui sera concernée par la rotation</param>
    /// <param name="clockwise">Si la rotation est dans le sens des aiguilles d'une poutre</param>
    private void ExecuteRotation(EditorCubeAxis axis, int row, bool clockwise)
    {
        Debug.Log("ROTATION " + axis + " ROW " + row + " CLOCKWISE " + clockwise);
        
        switch (axis)
        {
            case EditorCubeAxis.Horizontal:
                switch (row)
                {
                    case 1:
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[4], rubiksMovement.LeftCenterCube, clockwise);
                        break;
                    case 2:
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[4], rubiksMovement.MiddleCenterCube, clockwise);
                        break;
                    case 3: 
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[4], rubiksMovement.RightCenterCube, clockwise);
                        break;
                }
                break;
            case EditorCubeAxis.Vertical:
                switch (row)
                {
                    case 1:
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[2], rubiksMovement.BottomCenterCube, clockwise);
                        break;
                    case 2:
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[2], rubiksMovement.MiddleCenterCube, clockwise);
                        break;
                    case 3: 
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[2], rubiksMovement.TopCenterCube, clockwise);
                        break;
                }
                break;
            case EditorCubeAxis.Depth:
                switch (row)
                {
                    case 1:
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[0], rubiksMovement.FrontCenterCube, clockwise);
                        break;
                    case 2:
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[0], rubiksMovement.MiddleCenterCube, clockwise);
                        break;
                    case 3: 
                        rubiksMovement.RotateInEditor(rubiksMovement.Axis[0], rubiksMovement.BackCenterCube, clockwise);
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
