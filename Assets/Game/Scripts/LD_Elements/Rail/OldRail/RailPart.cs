using UnityEngine;
using System.Collections.Generic;

public class RailPart : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints; // spline control points in local space

    public Vector3[] GetWorldPoints()
    {
        Vector3[] worldPoints = new Vector3[controlPoints.Length];
        for (int i = 0; i < controlPoints.Length; i++)
            worldPoints[i] = controlPoints[i].position;
        return worldPoints;
    }

    public Vector3 StartPoint => controlPoints[0].position;
    public Vector3 EndPoint => controlPoints[controlPoints.Length - 1].position;
}
