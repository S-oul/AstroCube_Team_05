using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [Header("SpeedZone")]
    [SerializeField] float newSpeedMultiplyer = 0.5f;

    PlayerMovement _playerMovement;
    CharacterController _characterController;

    FloatingZone _flotingZone;
    private void Start()
    {
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
    }
}
