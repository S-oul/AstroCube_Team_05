using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;
using static CameraFocusAttractor;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera vcam;
    [SerializeField] Camera overlayCamera;

    [SerializeField] CameraFocusAttractor cameraFocusAttractor;
    [SerializeField] float valueThatTriggersCamPan = 0.2f;

    [Header("SpeedZone")]
    [SerializeField] float newSpeedMultiplyer = 0.5f;

    [SerializeField] VolumeProfile vol;

    GameSettings _gameSettings;
    PlayerMovement _playerMovement;
    CharacterController _characterController;

    FloatingZone _flotingZone;

    [SerializeField] private Material portailInt_Material;
    [SerializeField] float fovMultiplier = 1.0f;

    private bool isInExitFocusState = false;
    private Coroutine _fovCoroutine;

    private void Awake()
    {
        if (vcam == null) Debug.LogWarning("Cinemachine Virtual Camera not found.");
        if (overlayCamera == null) Debug.LogWarning("Overlay Camera not found.");
        if (cameraFocusAttractor == null) Debug.LogWarning("CameraFocusAttractor not found.");
    }

    private void Start()
    {
        _gameSettings = GameManager.Instance.Settings;

        if (portailInt_Material == null)
            Debug.LogError("C_Min_Material is not assigned.");

        if (!vol)
            vol = GameObject.FindGameObjectWithTag("GlobalVol")?.GetComponent<VolumeProfile>();

        if (vol && vol.TryGet<ChromaticAberration>(out var ca))
            ca.intensity.Override(.1f);

        _playerMovement = GetComponent<PlayerMovement>();
        _characterController = GetComponent<CharacterController>();

        portailInt_Material.SetFloat("_C_Min", _gameSettings.C_MIN.Evaluate(1));
        if (vcam)
            vcam.m_Lens.FieldOfView = GameManager.Instance.CustomSettings.customFov;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VictoryZone"))
        {
            EventManager.TriggerPlayerWin();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("DeathZone"))
        {
            EventManager.Instance.TriggerPlayerLose();
        }
        else if (other.CompareTag("SlipperyZone"))
        {
            _playerMovement.SetSlippingState(true);
        }
        else if (other.CompareTag("SpeedZone"))
        {
            _playerMovement.SetSpeed(_playerMovement.defaultSpeed * newSpeedMultiplyer);
        }
        else if (other.CompareTag("GravityZone"))
        {
            _playerMovement.HasGravity = false;
            _flotingZone = other.transform.GetComponent<FloatingZone>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_flotingZone && other.CompareTag("GravityZone"))
            _characterController.Move(Vector3.up * _flotingZone.GravityForce * Time.deltaTime);

        if (other.CompareTag("ConveyerBelt"))
        {
            var belt = other.GetComponent<ConveyerBeltManager>();
            _playerMovement.SetExternallyAppliedMovement(belt.direction, belt.speed);
        }

        if (other.CompareTag("Portal"))
        {
            float toEvaluate = Vector3.Distance(transform.position, other.transform.position) / 4f;
            float cameraFOV = Mathf.Lerp(32f, GameManager.Instance.CustomSettings.customFov * fovMultiplier, _gameSettings.CurveFOV.Evaluate(toEvaluate));
            float overlayFOV = Mathf.Lerp(15f, 43f, _gameSettings.CurveFOV.Evaluate(toEvaluate));
            float chroma = Mathf.Lerp(.1f, 50f, _gameSettings.CurveAberration.Evaluate(toEvaluate));

            if (vol && vol.TryGet<ChromaticAberration>(out var ca))
                ca.intensity.Override(chroma);

            if (toEvaluate > valueThatTriggersCamPan && !isInExitFocusState)
            {
                cameraFocusAttractor.StopAllFocus();
                isInExitFocusState = true;
            }

            cameraFocusAttractor.StartContinuousFocus(new CameraFocusParameters
            {
                PointOfInterest = other.transform,
                InDuration = 0.05f,
                Strength = 1.5f,
                DoIn = true
            });

            portailInt_Material.SetFloat("_C_Min", _gameSettings.C_MIN.Evaluate(toEvaluate));
            SmoothCameraTransition(cameraFOV, 0.1f);

            if (Camera.allCameras.Length > 1)
                overlayCamera.fieldOfView = overlayFOV;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlipperyZone"))
            _playerMovement.SetSlippingState(false);

        if (other.CompareTag("SpeedZone"))
            _playerMovement.SetSpeedToDefault();

        if (other.CompareTag("GravityZone"))
        {
            _flotingZone = null;
            _playerMovement.HasGravity = true;
        }

        if (other.CompareTag("ConveyerBelt"))
            _playerMovement.SetExternallyAppliedMovement(Vector3.zero);

        if (other.CompareTag("Portal"))
        {
            SmoothCameraTransition(GameManager.Instance.CustomSettings.customFov, 1f);
            overlayCamera.fieldOfView = 43f;
            portailInt_Material.SetFloat("_C_Min", _gameSettings.C_MIN.Evaluate(1));

            if (vol && vol.TryGet<ChromaticAberration>(out var ca))
                ca.intensity.Override(.1f);

            cameraFocusAttractor.StopAllFocus();
            isInExitFocusState = false;
        }
    }

    void SmoothCameraTransition(float targetFOV, float duration)
    {
        if (_fovCoroutine != null)
            StopCoroutine(_fovCoroutine);

        _fovCoroutine = StartCoroutine(TransitionCameraFOV(targetFOV, duration));
    }

    IEnumerator TransitionCameraFOV(float targetFOV, float duration)
    {
        float startFOV = vcam.m_Lens.FieldOfView;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            vcam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        vcam.m_Lens.FieldOfView = targetFOV;
        _fovCoroutine = null;
    }

    // Deactivates CromaticAberration filter when exiting playmode. 
    private void OnApplicationQuit()
    {
        if (vol && vol.TryGet<ChromaticAberration>(out var ca))
        {
            ca.intensity.Override(0f);
        }
    }
}
