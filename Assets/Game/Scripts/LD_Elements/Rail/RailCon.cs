using UnityEngine;
using System.Collections.Generic;

public class RailCon : MonoBehaviour
{
    public float connectThreshold = 0.2f;
    private List<RailPart> railParts = new List<RailPart>();

    void Start()
    {
        railParts.AddRange(FindObjectsByType<RailPart>(FindObjectsSortMode.None));
        ConnectAll();
    }

    void ConnectAll()
    {
        foreach (var a in railParts)
        {
            foreach (var b in railParts)
            {
                if (a == b) continue;

                // Check if their endpoints are close enough
                if (Vector3.Distance(a.EndPoint, b.StartPoint) < connectThreshold)
                {
                    // We found a connection!
                    Debug.DrawLine(a.EndPoint, b.StartPoint, Color.green, 5f);
                    RailPathBuilder.Instance.Connect(a, b);
                }
                else if (Vector3.Distance(a.StartPoint, b.EndPoint) < connectThreshold)
                {
                    Debug.DrawLine(a.StartPoint, b.EndPoint, Color.cyan, 5f);
                    RailPathBuilder.Instance.Connect(b, a);
                }
            }
        }
    }
}
