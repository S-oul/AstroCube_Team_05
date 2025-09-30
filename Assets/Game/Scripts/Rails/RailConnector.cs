using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class RailConnector : MonoBehaviour
{
    [SerializeField] private Transform TestOBJ;

    [SerializeField] private List<Transform> _railPoint = new List<Transform>();
    [SerializeField] private Vector3[] _railPointVec3;

    public bool doRailLoop;

    private float _railLenght;


    private LineRenderer _lineRenderer;

    [Button("Update Line")]
    void _SetLineRendererPos()
    {
        _railPointVec3 = new Vector3[_railPoint.Count];
        int i = 0;
        foreach (Transform t in _railPoint)
        {
            _railPointVec3[i++] = t.position;
        }
        _lineRenderer.positionCount = _railPointVec3.Length;
        _lineRenderer.SetPositions(_railPointVec3);
    }

    private void Update()
    {
        _SetLineRendererPos();
    }


    public float _GetRailLenght()
    {
        float somme = 0;
        for (int i = 0; i < _railPointVec3.Length - 1; i++)
        {
            somme += Vector3.Distance(_railPointVec3[i], _railPointVec3[i + 1]);
        }
        return _railLenght = somme;
    }
    public float TestPos = 0;
    [Button("Test AT")]
    public void Test()
    {
        print(TestPos);
        TestOBJ.position = _GetObjPos(TestPos);
        print("pos : " + _GetObjPos(TestPos).x);
    }

    public Vector3 _GetObjPos(float pos)
    {
        float sommeBefore = 0f;

        for (int i = 0; i < _railPointVec3.Length - 1; i++)
        {
            float segLength = Vector3.Distance(_railPointVec3[i], _railPointVec3[i + 1]);

            if (sommeBefore + segLength >= pos)
            {
                float localPos = (pos - sommeBefore) / segLength; // ratio mgl
                return Vector3.Lerp(_railPointVec3[i], _railPointVec3[i + 1], localPos);
            }

            sommeBefore += segLength;
        }

        return _railPointVec3[_railPointVec3.Length - 1];
    }

    private void OnValidate()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _SetLineRendererPos();
        print(_GetRailLenght());
    }

}
