using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamFocus : MonoBehaviour
{
    [SerializeField] private CameraFocusAttractor _cameraFocusAttractor;
    [SerializeField] private CameraFocusAttractor.CameraFocusParameters _parameters;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_cameraFocusAttractor != null)
            {
                _cameraFocusAttractor.StartFocus(_parameters);
            }
            else
            {
                Debug.LogWarning("TriggerCamFocus: CameraFocusAttractor is not assigned.");
            }
        }
    }
}
