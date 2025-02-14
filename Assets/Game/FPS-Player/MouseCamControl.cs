using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamControl : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _mouseCamControlSpeed = 100f;


    [Header ("Raycast")]
    [SerializeField] RubiksCubeController rubiksCubeController;
    [SerializeField] LayerMask _detectableLayer;
    [SerializeField] float _maxDistance;


    float _xRotation;

    RaycastHit _raycastInfo;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Camera movement
        float mouseX = Input.GetAxis("Mouse X") * _mouseCamControlSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseCamControlSpeed * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _playerTransform.Rotate(Vector3.up * mouseX);


        //Raycast
        if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableLayer))
        {
            Debug.Log(_raycastInfo.transform.name);
            rubiksCubeController.SetActualFace(_raycastInfo.transform.gameObject);
        }
    }
}
