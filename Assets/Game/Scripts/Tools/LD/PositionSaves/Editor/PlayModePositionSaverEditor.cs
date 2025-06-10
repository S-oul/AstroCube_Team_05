using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static PositionSaveFile;
using static UnityEngine.VFX.VFXTypeAttribute;

[CustomEditor(typeof(PlayModePositionSaver))]
public class PlayModePositionSaverEditor : Editor
{
    [SerializeField] Font _font;
    UnityEngine.SceneManagement.Scene _currentScene;

    public override void OnInspectorGUI()
    {
        if (_currentScene.name == null)
            _currentScene = EditorSceneManager.GetActiveScene();
        if (PlayModePositionSaver.PositionsSave == null || PlayModePositionSaver.PositionsSave.SceneName != _currentScene.name)
            GetSaveAsset();

        GUI.skin.font = _font;

        GUILayout.BeginVertical("GroupBox");

        GUI.backgroundColor = new Color(0.7f, 0.7f, 1f);
        if (GUILayout.Button("Save End/Right Positions", new GUIStyle(GUI.skin.button)))
        {
            SaveRightPositions();
        }        
        if (PlayModePositionSaver.PositionsSave != null)
        {
            GUI.backgroundColor = Color.white;
            GUILayout.Label($"RightActions saved : {PlayModePositionSaver.PositionsSave.RightActionInfos.Count}");
        }
        if (GUILayout.Button("Debug Objects", new GUIStyle(GUI.skin.button)))
        {
            DebugAllObjects();
        }
        GUILayout.EndVertical();

        GUI.backgroundColor = new Color(0.8f, 1f, 0.7f);
        if (GUILayout.Button("Save Positions", new GUIStyle(GUI.skin.button)))
        {
            SavePositions();
        }

        GUI.backgroundColor = new Color(1f, 0.9f, 0.5f);
        if (GUILayout.Button("Apply Saved Positions", new GUIStyle(GUI.skin.button)))
        {
            LoadPositions();
        }

        GUI.backgroundColor = new Color(1f, 0.7f, 0.8f);
        if (GUILayout.Button("Clear Positions", new GUIStyle(GUI.skin.button)))
        {
            ClearPosition();
        }

        if(PlayModePositionSaver.PositionsSave != null)
        {
            GUI.backgroundColor = Color.white;
            GUILayout.Label($"Positions saved : {PlayModePositionSaver.PositionsSave.Positions.Count}");
        }

        GUI.backgroundColor = new Color(0.6f, 0.8f, 1f);
        GUILayout.BeginHorizontal("GroupBox");

        GUILayout.Label("Save File : ");
        PlayModePositionSaver.PositionsSave = EditorGUILayout.ObjectField(PlayModePositionSaver.PositionsSave, typeof(PositionSaveFile), true) as PositionSaveFile;

        GUILayout.EndHorizontal();
    }


    private void SaveRightPositions()
    {
        EditorUtility.SetDirty(PlayModePositionSaver.PositionsSave);
        PlayModePositionSaver.PositionsSave.RightActionInfos = new();

        List<RightActionObject> allImportantObjects = ((PlayModePositionSaver)target).GetComponentsInChildren<RightActionObject>().ToList();

        foreach (RightActionObject o in allImportantObjects)
        {
            RightActionInfo info = new(o.gameObject, o.GetActualPose());
            o.RightPose = o.GetActualPose();
            PlayModePositionSaver.PositionsSave.RightActionInfos.Add(info);
        }

        AssetDatabase.SaveAssetIfDirty(PlayModePositionSaver.PositionsSave);
        EditorUtility.ClearDirty(PlayModePositionSaver.PositionsSave);
        
    }

    private void SavePositions()
    {
        EditorUtility.SetDirty(PlayModePositionSaver.PositionsSave);
        PlayModePositionSaver.PositionsSave.Objects = new();
        PlayModePositionSaver.PositionsSave.Positions = new();
        PlayModePositionSaver.PositionsSave.EulerAngles = new();
        PlayModePositionSaver.PositionsSave.Scales = new();

        List<Tile> allTiles = ((PlayModePositionSaver)target).GetComponentsInChildren<Tile>().ToList();
        allTiles.RemoveAt(0);

        for (int tile = 0; tile < allTiles.Count; tile++)
        {
            List<Transform> transforms = allTiles[tile].GetComponentsInChildren<Transform>().ToList();
            transforms.RemoveAt(0);
            foreach (Transform transform in transforms) if (transform != ((PlayModePositionSaver)target).transform)
                {
                    PlayModePositionSaver.PositionsSave.Objects.Add(transform.gameObject);
                    PlayModePositionSaver.PositionsSave.Positions.Add(transform.localPosition);
                    PlayModePositionSaver.PositionsSave.EulerAngles.Add(transform.localEulerAngles);
                    PlayModePositionSaver.PositionsSave.Scales.Add(transform.localScale);
                }
        }

        AssetDatabase.SaveAssetIfDirty(PlayModePositionSaver.PositionsSave);
        EditorUtility.ClearDirty(PlayModePositionSaver.PositionsSave);
    }

    private void LoadPositions()
    {
        for (int i = 0; i < PlayModePositionSaver.PositionsSave.Objects.Count; i++)
        {
            PlayModePositionSaver.PositionsSave.Objects[i].transform.localPosition = PlayModePositionSaver.PositionsSave.Positions[i];
            PlayModePositionSaver.PositionsSave.Objects[i].transform.localEulerAngles = PlayModePositionSaver.PositionsSave.EulerAngles[i];
            PlayModePositionSaver.PositionsSave.Objects[i].transform.localScale = PlayModePositionSaver.PositionsSave.Scales[i];
        }
    }

    private void ClearPosition()
    {
        EditorUtility.SetDirty(PlayModePositionSaver.PositionsSave);

        PlayModePositionSaver.PositionsSave.Objects = new();
        PlayModePositionSaver.PositionsSave.Positions = new();
        PlayModePositionSaver.PositionsSave.EulerAngles = new();
        PlayModePositionSaver.PositionsSave.Scales = new();

        AssetDatabase.SaveAssetIfDirty(PlayModePositionSaver.PositionsSave);
        EditorUtility.ClearDirty(PlayModePositionSaver.PositionsSave);
    }

    private void DebugAllObjects()
    {
        foreach (RightActionInfo a in PlayModePositionSaver.PositionsSave.RightActionInfos)
        {
            Debug.Log(a.ObjectName + " " + a.SiblingIndex);
        }
    }

    private void GetSaveAsset()
    {
        string path = "Assets/Resources";

        if (!AssetDatabase.IsValidFolder(path + "/CorrectActionsSaves"))
        {
            AssetDatabase.CreateFolder(path, "CorrectActionsSaves");
        }

        string name = "CorrectActionsSave_" + _currentScene.name;

        var _oldSave = AssetDatabase.LoadAssetAtPath(path + "/CorrectActionsSaves/" + name + ".asset", typeof(PositionSaveFile));
        if (_oldSave == null)
        {
            PlayModePositionSaver.PositionsSave = CreateInstance<PositionSaveFile>();
            PlayModePositionSaver.PositionsSave.SceneName = _currentScene.name;
            string newAssetPath = path + "/CorrectActionsSaves/" + name + ".asset";
            AssetDatabase.CreateAsset(PlayModePositionSaver.PositionsSave, newAssetPath);
            AssetDatabase.SaveAssets();
        }
        else
        {
            PlayModePositionSaver.PositionsSave = _oldSave as PositionSaveFile;
        }

        AssetDatabase.Refresh();
    }
}