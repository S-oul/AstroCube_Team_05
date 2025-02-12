using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] float _maxDistance;
    [SerializeField] LayerMask _detectableLayer;
    
    RaycastHit _raycastInfo;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableLayer))
        {
            Debug.Log(_raycastInfo.transform.name);
        }
    }
}
