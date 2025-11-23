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
    [SerializeField] private string terrainSwitch;
    [SerializeField] private float footstepInterval = 0.5f;
    [SerializeField] private bool playFootsteps = true;

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
        
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10, LayerMask.GetMask("Floor")))
        {
            FloorType floorType;
            hit.collider.TryGetComponent<FloorType>(out floorType);
            if (floorType == null) return;
            string detectedTag = floorType.FloorTypeTag;
            
            FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(footstepFmodEvent);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
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