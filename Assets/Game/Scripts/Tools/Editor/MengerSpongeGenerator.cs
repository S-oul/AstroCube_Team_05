using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MengerSpongeGenerator : MonoBehaviour
{
    [SerializeField, UnityEngine.Range(0, 5)] int DEPTH;
    [SerializeField] bool SaveSpongeAsFile;


    MeshRenderer _meshRender;
    MeshFilter _meshFilter;

    Mesh _mesh;
    List<Vector3> _vertexs = new List<Vector3>();
    List<int> _faces = new List<int>();
    List<Vector2> _uvs = new List<Vector2>();
    List<Vector3> _normals = new List<Vector3>();

    HashSet<Vector3Int> _AllCupePositions = new HashSet<Vector3Int>();
    float _AllCupeSize;

    int QuadIterator = 0;

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
        DrawMangerSponge(DEPTH, 50, Vector3Int.zero); // fills _AllCubePositions & _AllCubeSize
        DrawSubCubes();

        _mesh = new Mesh();
        _mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        _mesh.vertices = _vertexs.ToArray();
        _mesh.triangles = _faces.ToArray();
        _mesh.uv = _uvs.ToArray();
        _mesh.normals = _normals.ToArray();
        _meshFilter.mesh = _mesh;

        Debug.Log("Number of Vertexs : " + _vertexs.Count());

        if (DEPTH > 4)
        {
            Debug.LogError($"Mesh too large! Will not save to file.");
            return;
        }

        if (SaveSpongeAsFile) SaveMeshToFile("Assets/Game/Art/SpongeMeshes");
    }

    private void DrawSubCubes()
    {
        int i = 0;
        foreach (Vector3Int cubePos in _AllCupePositions)
        {
            Debug.Log($"Drawing Cubes: {i} / {_AllCupePositions.Count}");
            i++;

            DrawCubeMesh(_AllCupeSize, cubePos);
        }
    }

    private void SaveMeshToFile(string folderPath)
    {
        if (Directory.Exists(folderPath) == false)
        {
            Directory.CreateDirectory(folderPath);
        }

        string assetPath = $"{folderPath}/spongeFaceOpt{DEPTH}.asset";

        AssetDatabase.CreateAsset(_mesh, assetPath);
        AssetDatabase.SaveAssets();
        Debug.Log($"Saved new mesh at {assetPath}");
    }

    private void DrawMangerSponge(int depth, float size, Vector3Int pos)
    {
        if (depth == 0)
        {
            _AllCupePositions.Add(pos);
            if (_AllCupeSize != size) _AllCupeSize = size;
            //DrawSquareMesh(size, pos);
            return;
        }

        int step = (int)Mathf.Pow(3, depth - 1);

        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                for (int z = 0; z < 3; z++)
                {
                    int holes = (x == 1 ? 1 : 0) + (y == 1 ? 1 : 0) + (z == 1 ? 1 : 0);
                    if (holes >= 2) continue; // skip center holes

                    DrawMangerSponge(depth - 1, size / 3f, pos + new Vector3Int(x * step, y * step, z * step));
                }
    }

    void DrawQuadMesh(float size, Vector3 pos, Vector3 direction)
    {
        // create vertexs

        Vector3 right = Vector3.Cross(direction, Vector3.up);
        if (right == Vector3.zero) right = Vector3.Cross(direction, Vector3.forward);
        Vector3 up = Vector3.Cross(right, direction);

        Vector3[] vertices = new Vector3[4]
        {
            pos + (-right - up) * size * 0.5f,
            pos + ( right - up) * size * 0.5f,
            pos + (-right + up) * size * 0.5f,
            pos + ( right + up) * size * 0.5f
        };

        // create UV values
        Vector2[] uvs = new Vector2[4] 
        {
            new Vector2(0, 0),
            new Vector2(1f, 0),
            new Vector2(0, 1f),
            new Vector2(1f, 1f)
        };


        // create Normal values
        Vector3[] normals = new Vector3[4]
        {
            direction,
            direction,
            direction,
            direction
        };

        // create faces (tryangles)
        int[] faces = new int[6]
        {
            QuadIterator + 0, QuadIterator + 2, QuadIterator + 1,
            QuadIterator + 2, QuadIterator + 3, QuadIterator + 1
        };
        QuadIterator+=4;

        _vertexs.AddRange(vertices);
        _normals.AddRange(normals);
        _uvs.AddRange(uvs);
        _faces.AddRange(faces);
    }

    void DrawCubeMesh(float size, Vector3Int pos)
    {
        Vector3[] directions =
        {
            Vector3.up, Vector3.down,
            Vector3.left, Vector3.right,
            Vector3.forward, Vector3.back
        };

        Vector3Int[] cubeNeiborDirection =
        {
            Vector3Int.up, Vector3Int.down,
            Vector3Int.left, Vector3Int.right,
            Vector3Int.forward, Vector3Int.back
        };

        Vector3 worldPos = pos;
        worldPos *= size;

        for (int i = 0; i < 6; i++)
        {
            if (_AllCupePositions.Contains(pos + cubeNeiborDirection[i]) == false) // check if there is a cube next to it
            {
                DrawQuadMesh(size, worldPos+ (size/2) * directions[i], directions[i]);
            }
        }
    }
}
