using UnityEngine;

public class MouseCamControl : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _joyStCamControlSpeed = 1000f;
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Camera movement
        float moveX = Input.GetAxis("Joystick X") * _joyStCamControlSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Joystick X") * _joyStCamControlSpeed * Time.deltaTime;

        if (moveX == 0 && moveY == 0)
        {
            moveX = Input.GetAxis("Mouse X") * _mouseCamControlSpeed * Time.deltaTime;
            moveY = Input.GetAxis("Mouse Y") * _mouseCamControlSpeed * Time.deltaTime * -1;
        }


        _xRotation -= moveY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _playerTransform.Rotate(Vector3.up * moveX);


        //Raycast
        if (Physics.Raycast(transform.position, transform.forward, out _raycastInfo, _maxDistance, _detectableLayer))
        {
            //Debug.Log(_raycastInfo.transform.name);
            if (rubiksCubeController != null)
            {
                rubiksCubeController.SetActualFace(_raycastInfo.transform.gameObject);
            }
        }
    }
}
