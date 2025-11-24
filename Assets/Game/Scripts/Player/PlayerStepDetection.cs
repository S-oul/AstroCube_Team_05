using System;
using UnityEngine;
using FMODUnity;

public class PlayerStepDetection : MonoBehaviour
{
    [Header("FMOD Audio")]
    [SerializeField] private EventReference footstepFmodEvent;
    [SerializeField] private EventReference jumpFmodEvent;
    [SerializeField] private EventReference landFmodEvent;

    [Header("Footstep Settings")]
    [SerializeField] private string terrainSwitch = "PR_FT";
    [SerializeField] private float footstepInterval = 0.5f;
    [SerializeField] private bool playFootsteps = true;
    [SerializeField] private float groundCheckDistance = 3.0f; // Distance to check for ground

    private CharacterController _characterController;
    private Vector3 _lastPosition;

    private float timer = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        float velocity = (currentPosition - _lastPosition).magnitude;
        _lastPosition = currentPosition;
        
        if (!playFootsteps || !_characterController)
            return;
        
        if (velocity < 0.05f)
            return;

        timer += Time.deltaTime;

        if (timer >= footstepInterval)
        {
            PlayFootstep();
            timer = 0f;
        }
    }

    private void PlayFootstep()
    {
        if (footstepFmodEvent.IsNull)
            return;

        // Include both "Floor" and "Tile" layers
        int layerMask = LayerMask.GetMask("Floor", "Tile");
        
        // Start the ray slightly above the pivot to ensure we don't start inside the floor collider
        Vector3 startPoint = transform.position + transform.up * 0.5f;
        
        // Use the raycast itself to check if we're grounded - if we hit something close, we're on the ground
        if (Physics.Raycast(startPoint, -transform.up, out RaycastHit hit, groundCheckDistance, layerMask))
        {
            FloorType floorType;
            hit.collider.TryGetComponent<FloorType>(out floorType);
            
            if (floorType == null) 
            {
                // Try looking in parent if not found on the collider itself
                floorType = hit.collider.GetComponentInParent<FloorType>();
            }

            string detectedTag = "Concrete"; // Default fallback
            
            if (floorType != null) 
            {
                // The FloorTypeTag property now returns the Enum string (Carpet, Tiles, Dirt, Concrete)
                detectedTag = floorType.FloorTypeTag;
            }

            FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(footstepFmodEvent);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
            
            if (string.IsNullOrEmpty(terrainSwitch))
            {
                terrainSwitch = "PR_FT";
            }

            instance.setParameterByNameWithLabel(terrainSwitch, detectedTag);
            instance.start();
            instance.release();
        }
    }

    public void Jump()
    {
        if (!jumpFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(jumpFmodEvent, transform.position);
    }

    public void Land()
    {
        if (!landFmodEvent.IsNull)
            RuntimeManager.PlayOneShot(landFmodEvent, transform.position);
    }
}