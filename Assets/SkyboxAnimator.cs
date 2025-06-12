using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxAnimator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10.0f; // degrees per second
    [SerializeField] private float _startAngle = 180.0f; 

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", (Time.time * rotationSpeed)+_startAngle);
    }
}
