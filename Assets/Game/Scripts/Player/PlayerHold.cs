using UnityEngine;

public class PlayerHold : MonoBehaviour
{
    public bool IsHolding => _isHolding;
    private bool _isHolding;

    private IHoldable _holdedObject;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _rayDistance;
    [SerializeField] private Transform _holdTransform;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        _isHolding = false;
    }


    public void TryHold()
    {
        RaycastHit _raycastInfo;

        Debug.DrawRay(transform.position, _camera.transform.forward * _rayDistance, Color.red, 3);

        if (Physics.Raycast(transform.position, _camera.transform.forward, out _raycastInfo, _rayDistance, _layerMask))
        {
            IHoldable holdable = _raycastInfo.collider.transform.GetComponent<IHoldable>();

            if (holdable != null) 
            {
                EventManager.TriggerPlayerInteract();
                _holdedObject = holdable;
                _Hold();            
            }           
        }
    }


    public void TryRelease()
    {
        if (_holdedObject == null)
            return;
        _Release();
    }

    private void _Hold()
    {
        _isHolding = true;
        _holdedObject.OnHold(_holdTransform);
    }

    private void _Release()
    {
        _holdedObject.GetTransform().parent = _holdedObject.GetOriginalParent();
        _isHolding = false;
        _holdedObject.OnRelease();
        _holdedObject = null;
    }
}
