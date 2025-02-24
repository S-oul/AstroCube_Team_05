
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.InspectorCurveEditor;

[CustomEditor(typeof(RubicsCube)), CanEditMultipleObjects]
public class LevelSelection : Editor
{
    [SerializeField]
    SerializedProperty _tilesPerFaces;
    [SerializeField] Font font;
    static bool _isInIsolatedMode;
    static GUIStyle _buttonStyle;
    static GUIStyle _buttonStyle1;
    static bool _isObjectSelected;
    static string labelText;
    
    static RubicsCube _script;


    private void OnEnable()
    {
        _isInIsolatedMode = false;
        _isObjectSelected = false;
        _script = (RubicsCube)target;
    }

    private void OnSceneGUI()
    {
        if (RubicsCube.Selection[0] != null)
        {
            Handles.DrawOutline(RubicsCube.Selection, Color.yellow, 1);
            Handles.PositionHandle(RubicsCube.Selection[0].transform.position, Quaternion.identity);
        }

        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0);
        }

        if (Event.current.type == EventType.MouseDown)
        {

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit hit;
            var l = GetLayerMaskFromMode(RubicsCube.IsolationMode);
            if (Physics.Raycast(ray, out hit, 500, l))
            {
                if(_isObjectSelected && RubicsCube.Selection[0] != hit.collider.gameObject || _isInIsolatedMode)
                {
                    _script.DeIsolate(RubicsCube.IsolationMode, RubicsCube.Selection[0].transform, _script.transform);
                    _isInIsolatedMode = false;
                }
                RubicsCube.Selection[0] = hit.collider.gameObject;
                _isObjectSelected = true;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        RubicsCube data = (RubicsCube)target;

        GUI.skin.font = font;

        EditorGUILayout.Space(10);

        InitStyles();

        ActionsDisplay();

        /*
        for (int face = 0; face < 6; face++)
        {
            for (int tile = 0; tile < 9; tile++)
            {
                data.TilesPerFaces[face, tile] = (GameObject)EditorGUILayout.ObjectField("Face "+face+ " - Tile " + tile + " :", data.TilesPerFaces[face, tile], typeof(GameObject), true);
            }
        }
        */

        if(GUI.changed)
            AssetDatabase.SaveAssetIfDirty(target);

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    static void InitStyles()
    {
        _buttonStyle = GUI.skin.button;
        _buttonStyle.fontSize = 13;
        _buttonStyle.fontStyle = FontStyle.Bold;
        _buttonStyle.normal.textColor = !_isInIsolatedMode ? Color.black : Color.grey;
        _buttonStyle.hover.textColor = !_isInIsolatedMode ? Color.white : Color.black;

        _buttonStyle1 = GUI.skin.button;
        _buttonStyle1.fontSize = 13;
        _buttonStyle1.fontStyle = FontStyle.Normal;
        _buttonStyle1.normal.textColor = Color.black;
        _buttonStyle1.hover.textColor = Color.white;
    }

    static void ActionsDisplay()
    {
        InitStyles();
        GUILayout.Label("Level Editor ", new GUIStyle(GUI.skin.label) { fontSize = 15, normal = new GUIStyleState() { textColor = Color.white } });
        GUILayout.Space(10);


        GUILayout.BeginVertical("GroupBox");


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Isolation mode : ", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Normal, fontSize = 13, normal = new GUIStyleState() { textColor = new Color(0.9f, 0.8f, 0.6f) } });


        RubicsCube.IsolationMode = (EIsolationMode)EditorGUILayout.EnumPopup(RubicsCube.IsolationMode);

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.BeginVertical("GroupBox");

        labelText = RubicsCube.Selection[0] != null ? "Selected Object : " + RubicsCube.Selection[0].name : "Selected Object : No Object Selected";
        GUILayout.Label(labelText, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Normal, fontSize = 13, normal = new GUIStyleState() { textColor = new Color(0.6f, 0.7f, 1f) } });

        GUILayout.Space(10);

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = !_isInIsolatedMode ? Color.white : Color.grey;

        if (GUILayout.Button("Isolate", _buttonStyle, GUILayout.Height(25)))
        {
            if (RubicsCube.Selection[0] != null)
            {
                _isInIsolatedMode = !_isInIsolatedMode;

                if (_isInIsolatedMode)
                    _script.Isolate(RubicsCube.IsolationMode, RubicsCube.Selection[0].transform, _script.transform);
                else
                    _script.DeIsolate(RubicsCube.IsolationMode, RubicsCube.Selection[0].transform, _script.transform);
            }

        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = new Color(1f, 0.9f, 0.8f);
        if (GUILayout.Button("Go to selected object", _buttonStyle1, GUILayout.Height(25)))
        {
            if(RubicsCube.Selection[0] != null)
                UnityEditor.Selection.activeGameObject = RubicsCube.Selection[0];
        }

        GUI.backgroundColor = new Color(0.7f, 0.9f, 1f);
        if (GUILayout.Button("Reset Editor", _buttonStyle1, GUILayout.Height(25)))
        {
            _script.Reset();
            _isInIsolatedMode = false;
        }
        GUI.backgroundColor = Color.white;

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        GUILayout.EndVertical();

        GUILayout.EndVertical();
        GUILayout.EndVertical();
    }

    LayerMask GetLayerMaskFromMode(EIsolationMode mode)
    {
        switch (mode)
        {
            default:
            case EIsolationMode.CUBE:
            {
                LayerMask mask = LayerMask.GetMask("Cube");
                return mask;
            }
            case EIsolationMode.TILE:
            {
                LayerMask mask = LayerMask.GetMask("Tile");
                return mask;
            }
        }
    }
}
