using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour
{
    Vector2 localPosOnFace;

    //Local
    public bool Up;
    public bool Left;
    public bool Down;
    public bool Right;

    public Vector2 LocalPosOnFace { get => localPosOnFace; set => localPosOnFace = value; }

    [Button]
    public void DoSomething()
    {
        List<Transform> all =new List<Transform>(this.GetComponentsInChildren<Transform>(true));
        all.RemoveAt(0);

        all[0].gameObject.SetActive(Up);
        all[1].gameObject.SetActive(Left);
        all[2].gameObject.SetActive(Down);
        all[3].gameObject.SetActive(Right);

    }

    MapCell()
    {

    }

}
