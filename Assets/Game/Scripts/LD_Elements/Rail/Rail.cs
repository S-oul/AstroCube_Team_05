using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool IsLoop;
    private float length;
    private List<Transform> nodes = new();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        length = 0;
        nodes.Clear();
        int children = transform.childCount;
        for (int i = 0; i < children; i++)
        {
            nodes.Add(transform.GetChild(i));
            if (nodes.Count > 1)
                length += Vector3.Distance(transform.GetChild(i - 1).position, transform.GetChild(i).position);
        }
        if (IsLoop && transform.childCount > 1)
            length += Vector3.Distance(transform.GetChild(0).position, transform.GetChild(transform.childCount-1).position);
    }

    public float GetLength() => length;
    public Vector3 GetPosition(float distance)
    {
        if (distance > length && !IsLoop)
            return (GetPosition(length));

        if (distance < 0 && IsLoop)
        {
            distance = (distance % length + length) % length; ;
        }

        int i = 0;
        int y = 1;
        float currentDistance = 0f;
            
        while (true) {

            if (currentDistance + Vector3.Distance(nodes[i].position, nodes[y].position) < distance) { 
                currentDistance += Vector3.Distance(nodes[i].position, nodes[y].position);
            }
            else
            {
                float dis = distance - currentDistance;

                var percent = (distance - currentDistance) / Vector3.Distance(nodes[i].position, nodes[y].position);

                return Vector3.Lerp(nodes[i].position, nodes[y].position, percent);
            }

            i++;
            y++;

            if (!IsLoop)
                continue;
            if (i == nodes.Count)
                i %= nodes.Count;
            if (y == nodes.Count)
                y %= nodes.Count;
        }
    }

    private void OnDrawGizmos()
    {
        Init();
        Gizmos.color = Color.magenta;

        for (int i = 0; i < nodes.Count; i++)
        {
            Gizmos.DrawSphere(nodes[i].position, 0.1f);

            if (i > 0)
                Gizmos.DrawLine(nodes[i].position, nodes[i - 1].position);
        }
        if (IsLoop && nodes.Count > 1)
            Gizmos.DrawLine(nodes[0].position, nodes[^1].position);
    }

    public Vector3 GetClosestPointOnRail(Vector3 target) 
    {
        Vector3 result = Vector3.zero;
        float smallestDistance = float.MaxValue;

        for (int i = 0; i< nodes.Count; i++)
        {
            if (i < 1)
                continue;

            Vector3 closestPoint = GetNearestPointOnSegment(nodes[i-1].position, nodes[i].position, target);
            float distance = Vector3.Distance(closestPoint, target);

            if(distance < smallestDistance )
            {
                smallestDistance = distance;
                result = closestPoint;
            }           
        }
        return result;
    }

    public int IsOnNode(Vector3 position)
    {
        for (int n = 0; n < nodes.Count; n++)
        {
            if (Vector3.Distance(nodes[n].position, position) <= Mathf.Epsilon)
            {
                return n;
            }
        }
        return -1;
    }

    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 n = (b - a).normalized;
        float p = Vector3.Dot(c - a, n);
        p = Mathf.Clamp(p, 0, Vector3.Distance(a, b));
        return a + n * p;
    }
}
