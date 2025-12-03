using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SkyboxAnimator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float startAngle = 180f;

    [Header("HDRP Volume")]
    [SerializeField] private Volume volume;  // Assign in Inspector!

    private HDRISky hdriSky;

    private void Start()
    {
        if (volume == null)
        {
            Debug.LogError("[SkyboxAnimator] No Volume assigned.");
            enabled = false;
            return;
        }

        // Try to fetch HDRI Sky override
        if (!volume.profile.TryGet(out hdriSky))
        {
            Debug.LogError("[SkyboxAnimator] No HDRISky override found in the Volume.");
            enabled = false;
            return;
        }

        // Enable override if not already
        hdriSky.rotation.overrideState = true;

        // Set initial angle
        hdriSky.rotation.value = startAngle;
    }

    private void Update()
    {
        if (hdriSky == null)
            return;

        hdriSky.rotation.value = (startAngle + Time.time * rotationSpeed) % 360f;
    }
}
