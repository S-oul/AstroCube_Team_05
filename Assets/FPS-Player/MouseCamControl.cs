using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamControl : MonoBehaviour
{

    [SerializeField] float _mouseCamControlSpeed = 100f;
    [SerializeField] Transform _playerTransform;

    float _xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseCamControlSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseCamControlSpeed * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _playerTransform.Rotate(Vector3.up * mouseX);
    }
}
