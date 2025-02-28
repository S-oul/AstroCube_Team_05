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
    [SerializeField] private float stopThreshold = 0.01f;

    private GameObject currentCube = null;
    private bool cubeLocked = false; 

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
            cubeLocked = false; 
        }

        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the button");
            EventManager.TriggerButtonPressed();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            if (currentCube == collision.gameObject)
            {
                currentCube = null;
                cubeLocked = false;
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
        if (currentCube != null && !cubeLocked)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, currentCube.transform.position.y, transform.position.z);

            float distance = Vector3.Distance(new Vector3(currentCube.transform.position.x, 0, currentCube.transform.position.z),
                                              new Vector3(targetPosition.x, 0, targetPosition.z));

            float rotationDifference = Quaternion.Angle(currentCube.transform.rotation, transform.rotation);

            if (distance < stopThreshold && rotationDifference < stopThreshold)
            {
                cubeLocked = true;
                currentCube.transform.position = targetPosition;
                currentCube.transform.rotation = transform.rotation; 
            }
            else
            {
                currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, targetPosition, Time.deltaTime * magnetSpeed);

                currentCube.transform.rotation = Quaternion.Lerp(currentCube.transform.rotation, transform.rotation, Time.deltaTime * rotationSpeed);
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
