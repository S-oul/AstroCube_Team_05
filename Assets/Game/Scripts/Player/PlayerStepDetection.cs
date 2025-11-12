using System;
using UnityEngine;

public class PlayerStepDetection : MonoBehaviour
{
    [Header("Wwise Audio")]
    [SerializeField] private AK.Wwise.Event footstepWwiseEvent;
    [SerializeField] private AK.Wwise.Event jumpWwiseEvent;
    [SerializeField] private AK.Wwise.Event landWwiseEvent;

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
        if (footstepWwiseEvent == null)
            return;
        
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10, LayerMask.GetMask("Floor")))
        {
            string detectedTag = hit.collider.GetComponent<FloorType>().FloorTypeTag;
            AkSoundEngine.SetSwitch(terrainSwitch, detectedTag, gameObject);
            footstepWwiseEvent.Post(gameObject);
        }
    }

    public void Jump()
    {
        jumpWwiseEvent?.Post(gameObject);
    }

    public void Land()
    {
        landWwiseEvent?.Post(gameObject);
    }
}