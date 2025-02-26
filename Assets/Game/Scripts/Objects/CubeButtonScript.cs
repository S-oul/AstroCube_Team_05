using System.Collections;
using UnityEngine;

public class CubeButtonScript : MonoBehaviour
{
    private bool isPressed = false;

    [SerializeField] private GameObject buttonVisual;
    [SerializeField] private GameObject buttonPressedVisual;
    [SerializeField] private GameObject lightsVisual;
    [SerializeField] private float magnetSpeed = 20f; 
    [SerializeField] private float rotationSpeed = 10f; 

    private GameObject currentCube = null;

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

            currentCube = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            if (currentCube == collision.gameObject)
            {
                currentCube = null;
            }

            isPressed = false;
            UpdateVisuals();
            EventManager.TriggerButtonReleased();
        }
    }

    private void Update()
    {
        if (currentCube != null)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, currentCube.transform.position.y, transform.position.z);
            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, targetPosition, Time.deltaTime * magnetSpeed);

            Quaternion targetRotation = transform.rotation;
            currentCube.transform.rotation = Quaternion.Lerp(currentCube.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void UpdateVisuals()
    {
        buttonVisual.SetActive(!isPressed);
        buttonPressedVisual.SetActive(isPressed);
        lightsVisual.SetActive(isPressed);
    }
}
