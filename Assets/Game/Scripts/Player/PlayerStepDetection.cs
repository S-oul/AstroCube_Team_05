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

    private float _timer;

    //Use this 
    public bool PlayFootsteps { get => playFootsteps; set => playFootsteps = value; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _lastPosition = transform.position;
        _timer = footstepInterval;
    }

    private void  FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        float velocity = (currentPosition - _lastPosition).magnitude;
        _lastPosition = currentPosition;
        
        if (!playFootsteps || !_characterController)
            return;
        
        if (velocity < 0.15f)
        {
            _timer = footstepInterval;
            return;
        }

        _timer += Time.deltaTime;

        if (_timer >= footstepInterval)
        {
            PlayFootstep();
            _timer = 0f;
        }
    }

    private void PlayFootstep()
    {
        if (footstepFmodEvent.IsNull)
            return;

        int layerMask = LayerMask.GetMask("Floor");
        
        Vector3 startPoint = transform.position + transform.up * 0.5f;
        
        if (Physics.Raycast(startPoint, -transform.up, out RaycastHit hit, groundCheckDistance, layerMask))
        {
            FloorType floorType;
            hit.collider.TryGetComponent(out floorType);
            
            if (!floorType) 
            {
                floorType = hit.collider.GetComponentInParent<FloorType>();
            }

            string detectedTag = "Concrete";
            if (floorType) 
            {
                detectedTag = floorType.FloorTypeTag;
            }

            FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(footstepFmodEvent);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
            
            if (string.IsNullOrEmpty(terrainSwitch))
            {
                terrainSwitch = "PR_FT";
            }
            
            //Debug.Log("Footstep on: " + detectedTag);

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