using UnityEngine;
using System.Collections.Generic;

public class RailPathBuilder : MonoBehaviour
{
    public static RailPathBuilder Instance;
    //a
    private List<List<RailPart>> connectedGroups = new List<List<RailPart>>();

    void Awake() => Instance = this;

    public void Connect(RailPart a, RailPart b)
    {
        // Find if either part is already in a group
        List<RailPart> groupA = null, groupB = null;

        foreach (var g in connectedGroups)
        {
            if (g.Contains(a)) groupA = g;
            if (g.Contains(b)) groupB = g;
        }

        if (groupA == null && groupB == null)
        {
            // New group
            connectedGroups.Add(new List<RailPart> { a, b });
        }
        else if (groupA != null && groupB == null)
        {
            groupA.Add(b);
        }
        else if (groupB != null && groupA == null)
        {
            groupB.Add(a);
        }
        else if (groupA != groupB)
        {
            // Merge groups
            groupA.AddRange(groupB);
            connectedGroups.Remove(groupB);
        }
    }

    public List<Vector3> GetMergedPathPoints(List<RailPart> group)
    {
        List<Vector3> points = new List<Vector3>();
        foreach (var rail in group)
        {
            points.AddRange(rail.GetWorldPoints());
        }
        return points;
    }
}
