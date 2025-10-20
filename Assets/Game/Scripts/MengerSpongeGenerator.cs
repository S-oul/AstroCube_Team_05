using AmplifyShaderEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MengerSpongeGenerator : MonoBehaviour
{
    [SerializeField] int DEPTH;
    
    MeshRenderer _meshRender;
    MeshFilter _meshFilter;

    List<Vector3> _vertexs;
    List<int> _faces;

    int ObjectNameIterator = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _meshRender = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        if (DEPTH > 6)
        {
            Debug.Log("Depth of 6 or more will not be permited (for the sake of your computer device and sanity");
            DEPTH = 5;
        }
        DrawMangerSponge(DEPTH, 10, Vector3.zero);
    }

    private void DrawMangerSponge(int depth, float size, Vector3 pos)
    {
        if (depth == 0)
        {
            DrawQuadMesh(size, size, pos);
            return;
        }
        // row 1
        DrawMangerSponge(depth - 1, size / 3, pos);
        DrawMangerSponge(depth - 1, size / 3, pos + Vector3.right * size / 3);
        DrawMangerSponge(depth - 1, size / 3, pos + (Vector3.right * size / 3) * 2);

        // row 2
        DrawMangerSponge(depth - 1, size / 3, pos + Vector3.up * size / 3);
        DrawMangerSponge(depth - 1, size / 3, pos + (Vector3.up * size / 3) + (Vector3.right * size / 3) * 2);

        // row 3
        DrawMangerSponge(depth - 1, size / 3, pos + (Vector3.up * size / 3) * 2);
        DrawMangerSponge(depth - 1, size / 3, pos + (Vector3.up * size / 3) * 2 + (Vector3.right * size / 3));
        DrawMangerSponge(depth - 1, size / 3, pos + (Vector3.up * size / 3) * 2 + (Vector3.right * size / 3) * 2);
    }

    void DrawQuadMesh(float width, float height, Vector3 pos)
    {
        Mesh mesh = new Mesh();
        
        Vector3[] vertices = new Vector3[4]
        {
            Vector3.zero,
            width*Vector3.right,
            height*Vector3.up,
            width*Vector3.right + height*Vector3.up,
        };
        mesh.vertices = vertices;

        int[] faces = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };
        mesh.triangles = faces;

        _meshFilter.mesh = mesh;

        CreateMeshObject(pos, mesh);
    }

    void CreateMeshObject(Vector3 pos, Mesh mesh)
    {
        GameObject obj = new GameObject("Quad_" + ObjectNameIterator);
        ObjectNameIterator++;

        obj.transform.position = pos;
        obj.transform.SetParent(transform);
        
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();

        mf.mesh = mesh;
        mr.sharedMaterial = _meshRender.material;
    }
}
