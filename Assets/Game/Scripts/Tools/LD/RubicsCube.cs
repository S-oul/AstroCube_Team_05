using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RubicsCube : MonoBehaviour
{

    [SerializeField]
    public GameObject[,] TilesPerFaces => _tilesPerFaces;
    private GameObject[,] _tilesPerFaces = new GameObject[6, 9];

    public static GameObject[] Selection {  get => _selection; set => _selection = value; }
    private static GameObject[] _selection = { null };

    public static EIsolationMode IsolationMode { get => _isolationMode; set => _isolationMode = value; }
    private static EIsolationMode _isolationMode = EIsolationMode.TILE;

    public void Isolate(EIsolationMode mode, Transform selection, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            switch (mode)
            {
                case EIsolationMode.CUBE:
                    if (i != selection.GetSiblingIndex())
                        parent.GetChild(i).gameObject.SetActive(false);
                    break;
                case EIsolationMode.TILE:
                    Transform selectionCube = selection.parent;
                    if (i != selectionCube.GetSiblingIndex())
                        parent.GetChild(i).gameObject.SetActive(false);
                    else
                    {
                        for (int j = 0; j < selectionCube.childCount; j++)
                        {
                            if (j != selection.GetSiblingIndex() && selectionCube.GetChild(j).gameObject.layer == LayerMask.NameToLayer("Tile"))
                                selectionCube.GetChild(j).gameObject.SetActive(false);
                        }
                    }
                    break;
            }
        }
    }

    public void DeIsolate(EIsolationMode mode, Transform selection, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            switch (mode)
            {
                case EIsolationMode.CUBE:
                    if (i != selection.GetSiblingIndex())
                        parent.GetChild(i).gameObject.SetActive(true);
                    break;
                case EIsolationMode.TILE:
                    Transform selectionCube = selection.parent;
                    if (i != selectionCube.GetSiblingIndex())
                        parent.GetChild(i).gameObject.SetActive(true);
                    else
                    {
                        for (int j = 0; j < selectionCube.childCount; j++)
                        {
                            if (j != selection.GetSiblingIndex() && selectionCube.GetChild(j).gameObject.layer == LayerMask.NameToLayer("Tile"))
                                selectionCube.GetChild(j).gameObject.SetActive(true);
                        }
                    }
                    break;
            }
        }
    }

    public void Reset()
    {
        if (Selection[0] != null)
            DeIsolate(_isolationMode, RubicsCube.Selection[0].transform, transform);
    }

    private void OnDestroy()
    {
        Reset();
    }

    private void OnApplicationQuit()
    {
        Reset();
    }

    private void OnDrawGizmos()
    {
        if (Selection[0] != null)
            Handles.DrawOutline(Selection, Color.red, 1);
    }
}
