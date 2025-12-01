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
            _mainCamera.transform.position + _mainCamera.transform.forward * _interactionDistance, Color.magenta,
            Time.deltaTime);
        
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit hit, _interactionDistance, LayerMask.GetMask("Interactable")))
        {
            Debug.Log(hit.collider.gameObject.name);
            _currentInteractable = hit.collider.GetComponent<IInteractable>();
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
