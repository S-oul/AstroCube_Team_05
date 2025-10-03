using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class RailConnector : MonoBehaviour
{
    [SerializeField] private Transform TestOBJ;

    [SerializeField] private List<Transform> _railPoint = new List<Transform>();
    [SerializeField] private Vector3[] _railPointVec3;

    private RailObject _objOnRail;

    public bool doRailLoop;

    private float _railLenght =1;


    private LineRenderer _lineRenderer;

    private void Update()
    {
        _SetLineRendererPos();
        _UpdateRailLenght(); // recalculate length
    }

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

    public float _UpdateRailLenght()
    {
        float somme = 0;
        for (int i = 0; i < _railPointVec3.Length - 1; i++)
        {
            somme += Vector3.Distance(_railPointVec3[i], _railPointVec3[i + 1]);
        }

        //update oj pos
        if (_railLenght != somme)
        {
            //crossProduct
            print("LENGHT IS DIFFERRENTE");
            _objOnRail.ObjRailPos = (_objOnRail.ObjRailPos * somme) / _railLenght;
            _objOnRail._UpdatePhysics();

        }

        return _railLenght = somme;
    }

    public RailObject ObjOnRail { get => _objOnRail; set => _objOnRail = value; }
    public float RailLenght { get => _railLenght; set => _railLenght = value; }

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

    public struct RailInfo
    {
        public Vector3 position;
        public Vector3 direction;    //slope dir // dir.y is the slope angle downward 
        public Vector3 nearestPoint; // Nearest previous rail point | this maybe usefull in an edghecase situation;
    }

    /// <summary>
    /// This function gives no fuck about you
    /// </summary>
    /// <param name="railPos">The position on the rail '_objRailPos'</param>
    /// <returns>a RailInfo with in it the new position, slope direction, and the NearestPoint (anchor of therail)</returns>
    public RailInfo _GetRailInfoAtPos(float railPos)
    {
        float totalDistance = 0f;

        for (int i = 0; i < _railPointVec3.Length - 1; i++)
        {
            Vector3 start = _railPointVec3[i];
            Vector3 end = _railPointVec3[i + 1];
            float segmentLength = Vector3.Distance(start, end);

            if (totalDistance + segmentLength >= railPos)
            {
                float t = (railPos - totalDistance) / segmentLength;
                Vector3 position = Vector3.Lerp(start, end, t);
                Vector3 direction = (end - start).normalized;

                return new RailInfo
                {
                    position = position,
                    direction = direction,
                    nearestPoint = start
                };
            }

            totalDistance += segmentLength;
        }

        // if rail obj is at the end of the rail;
        return new RailInfo
        {
            position = _railPointVec3[_railPointVec3.Length - 1],
            direction = Vector3.zero,
            nearestPoint = _railPointVec3[_railPointVec3.Length - 2]
        };
    }


    private void OnValidate()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _SetLineRendererPos();
    }

}
