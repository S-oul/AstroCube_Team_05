using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorWindowTitle(title = "CSV Editor", icon = "Assets/Game/Scripts/Tools/Editor/papier.png")]
public class CSVEditor : EditorWindow
{
    private List<TextAsset> _csvFiles = new();
    private Dictionary<TextAsset, List<List<string>>> _csvFileToList = new();
    private int _currentModifyingCSVId = 0;

    private MultiColumnHeaderState _multiColumnHeaderState;
    private CSVTreeView _treeView;
    private Vector2 _scrollPosition;
    
    // Barre de recherche
    private string _searchText = "";

    [MenuItem("Tools/Localization")]
    public static void Init()
    {
        CSVEditor window = GetWindowWithRect<CSVEditor>(new Rect(0, 0, 900, 600), true);
        window.Show();
    }

    private void OnEnable()
    {
        LoadCSVFiles();
    }

    private void LoadCSVFiles()
    {
        _csvFileToList.Clear();
        _csvFiles = Resources.LoadAll<TextAsset>("Localization").ToList();
        
        foreach (TextAsset textAsset in _csvFiles)
        {
            List<List<string>> csvAsList = new();
            string text = textAsset.text;
            string[] lines = text.Split('\n');
            
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                csvAsList.Add(line.Trim().Split(';').ToList());
            }
            
            _csvFileToList[textAsset] = csvAsList;
        }

        if (_csvFiles.Count > 0)
        {
            InitializeTreeView();
        }
    }

    private void InitializeTreeView()
    {
        if (_csvFiles.Count == 0) return;

        TextAsset currentCSV = _csvFiles[_currentModifyingCSVId];
        var csvData = _csvFileToList[currentCSV];

        if (csvData.Count == 0) return;

        var headerRow = csvData[0];
        var columns = new List<MultiColumnHeaderState.Column>();

        columns.Add(new MultiColumnHeaderState.Column
        {
            headerContent = new GUIContent(""),
            headerTextAlignment = TextAlignment.Center,
            canSort = false,
            width = 30,
            minWidth = 30,
            maxWidth = 30,
            autoResize = false,
            allowToggleVisibility = false
        });

        for (int i = 0; i < headerRow.Count; i++)
        {
            columns.Add(new MultiColumnHeaderState.Column
            {
                headerContent = new GUIContent(headerRow[i]),
                headerTextAlignment = TextAlignment.Left,
                canSort = false,
                width = 200,
                minWidth = 100,
                autoResize = true,
                allowToggleVisibility = false
            });
        }

        _multiColumnHeaderState = new MultiColumnHeaderState(columns.ToArray());
        var multiColumnHeader = new MultiColumnHeader(_multiColumnHeaderState);
        multiColumnHeader.ResizeToFit();

        _treeView = new CSVTreeView(new TreeViewState(), multiColumnHeader, csvData, OnDeleteRow, _searchText);
    }

    private void OnDeleteRow(int rowIndex)
    {
        if (_csvFiles.Count == 0) return;

        TextAsset currentCSV = _csvFiles[_currentModifyingCSVId];
        var csvData = _csvFileToList[currentCSV];

        int actualIndex = rowIndex;
        if (actualIndex > 0 && actualIndex < csvData.Count)
        {
            csvData.RemoveAt(actualIndex);
            InitializeTreeView();
        }
    }

    private void OnDestroy()
    {
        SaveAllCSV();
    }

    private void OnGUI()
    {
        DrawHeader();
        GUILayout.Space(20);
        DrawSearchBar();
        GUILayout.Space(10);
        DrawToolbar();
        DrawTreeView();
        DrawButtons();
    }

    private void DrawHeader()
    {
        GUILayout.Space(20);
        
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label(
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Game/Scripts/Tools/Editor/papier.png"),
                new GUIStyle(GUI.skin.label) { fixedHeight = 64, fixedWidth = 64 }
            );
            GUILayout.Space(20);
            GUILayout.Label(
                "CSV Editor",
                new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 30,
                    fontStyle = FontStyle.Bold,
                    fixedHeight = 64
                }
            );
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSearchBar()
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("Rechercher ID:", GUILayout.Width(100));
            
            EditorGUI.BeginChangeCheck();
            _searchText = EditorGUILayout.TextField(_searchText, EditorStyles.toolbarSearchField);
            if (EditorGUI.EndChangeCheck())
            {
                if (_treeView != null)
                {
                    _treeView.UpdateSearchFilter(_searchText);
                }
            }

            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                _searchText = "";
                if (_treeView != null)
                {
                    _treeView.UpdateSearchFilter(_searchText);
                }
                GUI.FocusControl(null);
            }
            
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawToolbar()
    {
        List<string> csvNames = _csvFiles.Select(file => file.name).ToList();
        int newSelection = GUILayout.Toolbar(_currentModifyingCSVId, csvNames.ToArray());

        if (newSelection != _currentModifyingCSVId)
        {
            _currentModifyingCSVId = newSelection;
            InitializeTreeView();
        }
    }

    private void DrawTreeView()
    {
        if (_treeView != null && _csvFiles.Count > 0)
        {
            Rect rect = GUILayoutUtility.GetRect(0, position.height - 250, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            _treeView.OnGUI(rect);
        }
    }

    private void DrawButtons()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Add Line", GUILayout.Height(30)))
            {
                AddNewLine();
            }

            if (GUILayout.Button("Save All", GUILayout.Height(30)))
            {
                SaveAllCSV();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddNewLine()
    {
        if (_csvFiles.Count == 0) return;

        TextAsset currentCSV = _csvFiles[_currentModifyingCSVId];
        var csvData = _csvFileToList[currentCSV];

        if (csvData.Count == 0) return;

        int columnCount = csvData[0].Count;
        List<string> newRow = new List<string>();
        for (int i = 0; i < columnCount; i++)
        {
            newRow.Add("");
        }

        csvData.Add(newRow);
        InitializeTreeView();
    }

    private void SaveAllCSV()
    {
        foreach (TextAsset csvFile in _csvFiles)
        {
            EditorUtility.SetDirty(csvFile);

            string newCSV = "";
            var currentCSV = _csvFileToList[csvFile];
            
            foreach (var lines in currentCSV)
            {
                newCSV += string.Join(";", lines) + "\n";
            }

            string path = AssetDatabase.GetAssetPath(csvFile);
            File.WriteAllText(path, newCSV);

            AssetDatabase.SaveAssetIfDirty(csvFile);
        }
        
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Create/Localization/Generate new CSV")]
    private static void GenerateNewCSVFile()
    {
        string pathFolder = "Assets/Game/Resources/Localization/";
        string toWrite = "ID;";
        foreach (ELanguage lang in Enum.GetValues(typeof(ELanguage)))
        {
            toWrite += lang.ToString() + ';';
        }

        toWrite.TrimEnd(';');
        
        using (StreamWriter sw = new StreamWriter(pathFolder + "/New CSV.csv"))
        {
            sw.WriteLine(toWrite);
        }
        
        AssetDatabase.Refresh();

        UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(pathFolder + "/New CSV.csv");
        Selection.activeObject = obj;
        EditorGUIUtility.PingObject(obj);
    }
}

public class CSVTreeView : TreeView
{
    private List<List<string>> _csvData;
    private Action<int> _onDeleteRow;
    private string _searchFilter = "";

    public CSVTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, List<List<string>> csvData, Action<int> onDeleteRow, string searchFilter = "")
        : base(state, multiColumnHeader)
    {
        _csvData = csvData;
        _onDeleteRow = onDeleteRow;
        _searchFilter = searchFilter;
        rowHeight = 20;
        showAlternatingRowBackgrounds = true;
        showBorder = true;
        Reload();
    }

    public void UpdateSearchFilter(string searchText)
    {
        _searchFilter = searchText;
        Reload();
    }

    protected override TreeViewItem BuildRoot()
    {
        var root = new TreeViewItem { id = -1, depth = -1, displayName = "Root" };
        var allItems = new List<TreeViewItem>();

        for (int i = 1; i < _csvData.Count; i++)
        {
            if (!string.IsNullOrEmpty(_searchFilter))
            {
                if (_csvData[i].Count > 0)
                {
                    string id = _csvData[i][0];
                    if (!id.ToLower().Contains(_searchFilter.ToLower()))
                    {
                        continue;
                    }
                }
            }

            var item = new CSVTreeViewItem(i, 0, i.ToString(), _csvData[i]);
            allItems.Add(item);
        }

        SetupParentsAndChildrenFromDepths(root, allItems);
        return root;
    }

    protected override void RowGUI(RowGUIArgs args)
    {
        var item = args.item as CSVTreeViewItem;
        if (item == null) return;

        for (int i = 0; i < args.GetNumVisibleColumns(); i++)
        {
            CellGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
        }
    }

    private void CellGUI(Rect cellRect, CSVTreeViewItem item, int columnIndex, ref RowGUIArgs args)
    {
        if (columnIndex == 0)
        {
            GUI.backgroundColor = Color.red;
            if (GUI.Button(cellRect, "×", new GUIStyle(GUI.skin.button) { fontSize = 16, fontStyle = FontStyle.Bold }))
            {
                _onDeleteRow?.Invoke(item.id);
            }
            GUI.backgroundColor = Color.white;
            return;
        }

        int dataColumnIndex = columnIndex - 1;
        if (dataColumnIndex >= item.Values.Count) return;

        EditorGUI.BeginChangeCheck();
        string newValue = EditorGUI.TextField(cellRect, item.Values[dataColumnIndex]);
        if (EditorGUI.EndChangeCheck())
        {
            item.Values[dataColumnIndex] = newValue;
        }
    }
}

public class CSVTreeViewItem : TreeViewItem
{
    public List<string> Values { get; set; }

    public CSVTreeViewItem(int id, int depth, string displayName, List<string> values) : base(id, depth, displayName)
    {
        Values = values;
    }
}