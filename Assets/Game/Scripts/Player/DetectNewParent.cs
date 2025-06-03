using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DetectNewParent : MonoBehaviour
{
    [SerializeField] private LayerMask _detectableLayer;
    [SerializeField] private float _detectionRadius = .3f;
    [SerializeField] private bool _doGroundRotation;

    private bool _doGravityRotation;

    public bool DoGravityRotation { get => _doGravityRotation; set => _doGravityRotation = value; }
    public SelectionCube CurrentParent { get; private set; }

    private void Awake()
    {
        EventManager.OnPlayerReset += DisableParentChangerfor;
        EventManager.OnPlayerResetOnce += DisableParentChangerfor;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerReset -= DisableParentChangerfor;
        EventManager.OnPlayerResetOnce -= DisableParentChangerfor;
    }

    private void Update()
    {
        SelectionCube hitSelection = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position - transform.up, _detectionRadius, _detectableLayer);
        if (colliders.Length != 0)
        {
            hitSelection = colliders[0].transform.GetComponentInParent<SelectionCube>();
            if (hitSelection == null)
                return;
            if (CurrentParent == null || CurrentParent.transform != hitSelection.transform)
            {
                CurrentParent = hitSelection;
                EventManager.TriggerPlayerChangeParent();   
            }            
        }

        if (_doGroundRotation && hitSelection)
        {
            transform.SetParent(CurrentParent.transform, true);
        }
    }

    public void ToggleParentChanger(bool isOn)
    {
        _doGroundRotation = isOn;
        if (!_doGroundRotation)
            transform.SetParent(null, true);
    }

    public void DisableParentChangerfor(float duration)
    {
        StartCoroutine(DisableParentChanger(duration));
    }
    IEnumerator DisableParentChanger(float duration)
    {
        _doGroundRotation = false;
        yield return new WaitForSeconds(duration);
        _doGroundRotation = true;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!_doGravityRotation) return;

        var h = hit.collider.transform;
        var dif = transform.up - (-h.right);
        if (Mathf.Abs(dif.magnitude) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, -h.right) * transform.rotation, 1f);
        }
        transform.SetParent(hit.collider.gameObject.transform);
    }
}
