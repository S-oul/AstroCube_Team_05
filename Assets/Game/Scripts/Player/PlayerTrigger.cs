using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera vcam;


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
    }


    private void Start()
    {

        _gameSettings = GameManager.Instance.Settings;

        cmin = _gameSettings.C_MIN;

        if (portailInt_Material == null)
        {
            Debug.LogError("C_Min_Material is not assigned in PlayerTrigger script.");
        }

        float materialCminValue = portailInt_Material.GetFloat("_C_Min");


        if (!vol) vol = GameObject.FindGameObjectWithTag("GlobalVol")?.GetComponent<VolumeProfile>();
        if (vol)
        {
            if (vol.TryGet<ChromaticAberration>(out var ca))
                ca.intensity.Override(.1f);
        }
        _playerMovement = GetComponent<PlayerMovement>();
        _characterController = GetComponent<CharacterController>();
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
            float cameraFOV = Mathf.Lerp(15, GameManager.Instance.CustomSettings.customFov, _gameSettings.CurveFOV.Evaluate(Vector3.Distance(this.transform.position, other.transform.position) / 4f));
            float cameraOverlayFOV = Mathf.Lerp(15, 43, _gameSettings.CurveFOV.Evaluate(Vector3.Distance(this.transform.position, other.transform.position) / 4f));

            float chromaticAbberation = Mathf.Lerp(.1f, 50, _gameSettings.CurveAberration.Evaluate(Vector3.Distance(this.transform.position, other.transform.position) / 4f));

            if (vol)
            {
                if (vol.TryGet<ChromaticAberration>(out var ca))
                    ca.intensity.Override(chromaticAbberation);
            }


            vcam.m_Lens.FieldOfView = cameraFOV;
            if (Camera.allCameras.Length > 1)
                vcam.m_Lens.FieldOfView = cameraOverlayFOV;

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

            if (vol)
            {
                if (vol.TryGet<ChromaticAberration>(out var ca))
                    ca.intensity.Override(.1f);
            }
        }
    }
}
