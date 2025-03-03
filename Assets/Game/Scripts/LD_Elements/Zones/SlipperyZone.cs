using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SlipperyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        other.gameObject.GetComponent<PlayerMovement>().SetSlippingState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        other.gameObject.GetComponent<PlayerMovement>().SetSlippingState(false);
    }
}
