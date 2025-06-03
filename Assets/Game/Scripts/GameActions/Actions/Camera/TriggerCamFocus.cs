using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamFocus : MonoBehaviour
{
    [SerializeField] private CameraFocusAttractor CameraFocusAttractor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CameraFocusAttractor != null)
            {
                CameraFocusAttractor.StartFocus();
            }
            else
            {
                Debug.LogWarning("TriggerCamFocus: CameraFocusAttractor is not assigned.");
            }
        }
    }
}
