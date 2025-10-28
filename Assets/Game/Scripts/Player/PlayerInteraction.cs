using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _interactionDistance;
    
    private Camera _mainCamera;
    private IInteractable _currentInteractable;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Debug.DrawLine(_mainCamera.transform.position,
            _mainCamera.transform.position + _mainCamera.transform.forward * _interactionDistance, Color.red,
            Time.deltaTime);
        
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit hit, _interactionDistance))
        {
            _currentInteractable = hit.transform.gameObject.GetComponent<IInteractable>();
        }
    }

    public void Interact()
    {
        _currentInteractable?.OnInteract();
    }
}

public interface IInteractable
{
    public void OnInteract();
}
