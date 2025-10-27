using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RailManager : MonoBehaviour
{
    static public RailManager instance;
    private void Awake()
    {
        if (instance) return;
        instance = this;
    }

    [SerializeField] List<RailGroup> listRailGroups = new List<RailGroup>();

    public List<RailGroup> ListRailGroups { get => listRailGroups; set => listRailGroups = value; }

    //Need to know the RailGroup
    public Vector3[] CreatePath(RailGroup railGroupPath)
    {
        if (railGroupPath == null || railGroupPath.allPositions == null || railGroupPath.allPositions.Count == 0)
            return System.Array.Empty<Vector3>();

        List<Vector3> result = new();

        foreach (Transform point in railGroupPath.allPositions)
        {
            if (point == null) continue;

            Vector3 pos = point.position;

            // Skip if too close to last added point
            /*if (result.Count > 0 && Vector3.Distance(pos, result[^1]) < 0.1f)
                continue;*/
            //Shortest path sort make this useless

            result.Add(pos);
        }

        return result.ToArray();
    }


    [Button("Create")]
    void CreatRailLineRenderer()
    {
        var lr = GetComponent<LineRenderer>();
        lr.positionCount = listRailGroups[0].allPositions.Count;
        lr.SetPositions(CreatePath(listRailGroups[0]));
    }
}

[Serializable]
public class RailGroup
{
    public List<RailDetector> detectors;
    public List<Transform> allPositions;

    public bool usable = false;
    public RailGroup()
    {
        usable = false;
        detectors = new();
        allPositions = new();
    }

    public void SortPositionsByNearest()
    {
        if (allPositions == null || allPositions.Count <= 2)
            return;

        List<Transform> sorted = new();
        HashSet<Transform> visited = new();

        Transform current = allPositions.FirstOrDefault(t => t.GetComponent<RailDetector>()?.imEnd == true);
        sorted.Add(current);
        visited.Add(current);

        while (sorted.Count < allPositions.Count)
        {
            Transform next = null;
            float nearest = float.MaxValue;

            foreach (var t in allPositions)
            {
                if (t == null || visited.Contains(t)) continue;

                float dist = Vector3.Distance(current.position, t.position);
                if (dist < nearest)
                {
                    nearest = dist;
                    next = t;
                }
            }

            if (next == null) break;
            sorted.Add(next);
            visited.Add(next);
            current = next;
        }

        allPositions = sorted;
    }

    public RailGroup(RailDetector r1, RailDetector r2)
    {
        usable = true;

        detectors = new();
        allPositions = new();

        AddToGroup(r1);
        AddToGroup(r2);


        r1.groupIsIn = this; r2.groupIsIn = this;

        SortPositionsByNearest();

        RailManager.instance.ListRailGroups.Add(this);
    }

    private void AddRailPositions(RailDetector detector)
    {
        foreach (Transform t in detector.RailPos)
        {
            if (!allPositions.Contains(t))
            {
                allPositions.Add(t);
                RailDetector v;
                if (t.TryGetComponent<RailDetector>(out v))
                {
                    if (!detectors.Contains(v))
                    {
                        detectors.Add(v);
                        v.groupIsIn = this;
                    }
                }
            }
        }
    }

    public void AddToGroup(RailDetector detector)
    {
        if (detectors.Contains(detector)) return;
        detectors.Add(detector);
        detector.groupIsIn = this;
        AddRailPositions(detector);

        SortPositionsByNearest();
    }



    public void RemoveFromGroup(RailDetector detector)
    {
        if (detector.groupIsIn != this) return;
        detectors.Remove(detector);
        detector.groupIsIn = null;
    }

    //shortest One If possible;
    public void MergeGroupe(ref RailGroup otherGroup)
    {
        if (otherGroup == null || otherGroup == this)
            return;

        var detectorsToAdd = new List<RailDetector>(otherGroup.detectors);
        foreach (RailDetector r in detectorsToAdd)
        {
            if (!detectors.Contains(r))
            {
                detectors.Add(r);
                r.groupIsIn = this;
                AddRailPositions(r);
            }
        }

        foreach (Transform t in otherGroup.allPositions)
        {
            if (!allPositions.Contains(t))
                allPositions.Add(t);
        }


        SortPositionsByNearest();


        RailManager.instance.ListRailGroups.Remove(otherGroup);

        otherGroup.detectors.Clear();
        otherGroup.allPositions.Clear();
        otherGroup.usable = false;
        otherGroup = null;
    }
}
