using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera vcam;
    [SerializeField] Camera overlayCamera;


    [Header("SpeedZone")]
    [SerializeField] float newSpeedMultiplyer = 0.5f;


    [SerializeField] VolumeProfile vol;

    GameSettings _gameSettings;

    PlayerMovement _playerMovement;
    CharacterController _characterController;

    FloatingZone _flotingZone;
    private float cmin;

    [SerializeField] private Material portailInt_Material;



    private void Awake()
    {
        if (vcam == null)
        {
            Debug.LogError("Cinemachine Virtual Camera not found in PlayerTrigger script.");
        }
        if (overlayCamera == null)
        {
            Debug.LogError("Overlay Camera not found in PlayerTrigger script.");
        }
    }


    private void Start()
    {

        _gameSettings = GameManager.Instance.Settings;


        if (portailInt_Material == null)
        {
            Debug.LogError("C_Min_Material is not assigned in PlayerTrigger script.");
        }
        
        if (!vol) vol = GameObject.FindGameObjectWithTag("GlobalVol")?.GetComponent<VolumeProfile>();
        if (vol)
        {
            if (vol.TryGet<ChromaticAberration>(out var ca))
                ca.intensity.Override(.1f);
        }
        _playerMovement = GetComponent<PlayerMovement>();
        _characterController = GetComponent<CharacterController>();
        
        
        //Set settings on Starts
        portailInt_Material.SetFloat("_C_Min", _gameSettings.C_MIN.Evaluate(1));
        vcam.m_Lens.FieldOfView = GameManager.Instance.CustomSettings.customFov;
        if (vol)
        {
            if (vol.TryGet<ChromaticAberration>(out var ca))
                ca.intensity.Override(.1f);
        }
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
        {
            _characterController.Move(Vector3.up * _flotingZone.GravityForce * Time.deltaTime);

        }

        if (other.gameObject.tag == "ConveyerBelt")
        {
            Vector3 dir = other.GetComponent<ConveyerBeltManager>().direction;
            float speed = other.GetComponent<ConveyerBeltManager>().speed;
            GetComponent<PlayerMovement>().SetExternallyAppliedMovement(dir, speed);
        }

        if (other.CompareTag("Portal"))
        {
            float toEvaluate = Vector3.Distance(this.transform.position, other.transform.position) / 4f;
            print(toEvaluate);
            float cameraFOV = Mathf.Lerp(15, GameManager.Instance.CustomSettings.customFov, _gameSettings.CurveFOV.Evaluate(toEvaluate));
            float cameraOverlayFOV = Mathf.Lerp(15, 43, _gameSettings.CurveFOV.Evaluate(toEvaluate));

            float chromaticAbberation = Mathf.Lerp(.1f, 50, _gameSettings.CurveAberration.Evaluate(toEvaluate));

            if (vol)
            {
                if (vol.TryGet<ChromaticAberration>(out var ca))
                    ca.intensity.Override(chromaticAbberation);
            }


            portailInt_Material.SetFloat("_C_Min", _gameSettings.C_MIN.Evaluate(toEvaluate));
            
            vcam.m_Lens.FieldOfView = cameraFOV;
            if (Camera.allCameras.Length > 1)
                overlayCamera.fieldOfView = cameraOverlayFOV;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlipperyZone"))
        {
            _playerMovement.SetSlippingState(false);
        }
        else if (other.CompareTag("SpeedZone"))
        {
            _playerMovement.SetSpeedToDefault();
        }
        else if (other.CompareTag("GravityZone"))
        {
            _flotingZone = null;
            _playerMovement.HasGravity = true;
        }

        if (other.gameObject.tag == "ConveyerBelt")
        {
            GetComponent<PlayerMovement>().SetExternallyAppliedMovement(Vector3.zero);
        }
        if (other.CompareTag("Portal"))
        {
            vcam.m_Lens.FieldOfView = GameManager.Instance.CustomSettings.customFov;
            portailInt_Material.SetFloat("_C_Min", _gameSettings.C_MIN.Evaluate(1));
            if (vol)
            {
                if (vol.TryGet<ChromaticAberration>(out var ca))
                    ca.intensity.Override(.1f);
            }
        }
    }
}
