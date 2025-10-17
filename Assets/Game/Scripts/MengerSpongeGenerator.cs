using AmplifyShaderEditor;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MengerSpongeGenerator : MonoBehaviour
{
    MeshRenderer _meshRender;
    MeshFilter _meshFilter;
    Mesh _mesh;

    List<Vector3> _vertexs;
    List<int> _faces;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _meshRender = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();

        _vertexs = new List<Vector3>();
        _faces = new List<int>();

        DrawQuadMesh(10, 10, Vector3.zero);
    }

    void DrawQuadMesh(float width, float height, Vector3 pos)
    {
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };
        _mesh.vertices = vertices;

        int[] faces = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };
        _mesh.triangles = faces;

        _meshFilter.mesh = _mesh;
    }
}
