using System.Collections;
using UnityEngine;

public class BoxButtonScript : MonoBehaviour
{
    private bool isPressed = false;

    [SerializeField] private GameObject buttonVisual;
    [SerializeField] private GameObject buttonPressedVisual;
    [SerializeField] private GameObject lightsVisual;
    [SerializeField] private float magnetSpeed = 20f; 
    [SerializeField] private float rotationSpeed = 10f; 
    [SerializeField] private float stopThreshold = 0.01f;

    private GameObject _currentBox = null;
    private bool _boxLocked = false; 

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            if (!isPressed)
            {
                isPressed = true;
                UpdateVisuals();
                EventManager.TriggerButtonPressed();
            }

            _currentBox = collision.gameObject;
            _boxLocked = false; 
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            EventManager.TriggerButtonPressed();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            if (_currentBox == collision.gameObject)
            {
                _currentBox = null;
                _boxLocked = false;
            }

            isPressed = false;
            UpdateVisuals();
            EventManager.TriggerButtonReleased();
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            EventManager.TriggerButtonReleased();
        }
    }

    private void Update()
    {
        if (_currentBox != null && !_boxLocked)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, _currentBox.transform.position.y, transform.position.z);

            float distance = Vector3.Distance(new Vector3(_currentBox.transform.position.x, 0, _currentBox.transform.position.z),
                                              new Vector3(targetPosition.x, 0, targetPosition.z));

            float rotationDifference = Quaternion.Angle(_currentBox.transform.rotation, transform.rotation);

            if (distance < stopThreshold && rotationDifference < stopThreshold)
            {
                _boxLocked = true;
                _currentBox.transform.position = targetPosition;
                _currentBox.transform.rotation = transform.rotation; 
            }
            else
            {
                _currentBox.transform.position = Vector3.Lerp(_currentBox.transform.position, targetPosition, Time.deltaTime * magnetSpeed);

                _currentBox.transform.rotation = Quaternion.Lerp(_currentBox.transform.rotation, transform.rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void UpdateVisuals()
    {
        buttonVisual.SetActive(!isPressed);
        buttonPressedVisual.SetActive(isPressed);
        lightsVisual.SetActive(isPressed);
    }
}
