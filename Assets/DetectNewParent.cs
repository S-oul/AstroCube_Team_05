using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectNewParent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer != "Floor") return;
        transform.parent.SetParent(other.transform);
        //Debug.Log("new parent named: " + other.gameObject.name);
    }
}
