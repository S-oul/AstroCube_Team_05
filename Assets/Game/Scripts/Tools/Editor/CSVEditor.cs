using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private void CreateGUI()
    {
        var textAssets = Resources.LoadAll<TextAsset>("Localization");
        foreach (TextAsset textAsset in textAssets)
        {
            string text = textAsset.text;
            string[] lines = text.Split('\n');
            foreach (string str in lines)
            {
                Debug.Log(str);
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
            GUILayout.Label("CSV Editor", new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter, fontSize = 30, fontStyle = FontStyle.Bold, fixedHeight = 64});
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }
    
    private static T[] GetAtPath<T> (string path) {
		
        ArrayList al = new ArrayList();
        string [] fileEntries = Directory.GetFiles(Application.dataPath+"/"+path);
        foreach(string fileName in fileEntries)
        {
            int index = fileName.LastIndexOf("/");
            string localPath = "Assets/" + path;
			
            if (index > 0)
                localPath += fileName.Substring(index);
				
            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

            if(t != null)
                al.Add(t);
        }
        T[] result = new T[al.Count];
        for(int i=0;i<al.Count;i++)
            result[i] = (T)al[i];
			
        return result;
    }
}
