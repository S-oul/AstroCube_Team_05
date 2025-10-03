using NaughtyAttributes;
using UnityEngine;
using static RailConnector;

public class RailObject : MonoBehaviour
{
    [Header("Physics Settings")]
    [SerializeField] private bool _frictionFree = false;
    [SerializeField] private float _gravityFactor = 1f;
    [SerializeField] private float _friction = 0.2f;
    [SerializeField] private float _maxMomentum = 20f;
    [SerializeField] private float _stopThreshold = 0.05f;

    [Header("Rail State")]
    [SerializeField] private RailConnector _railImOn;
    [SerializeField] private float _objRailPos = 0f;

    [ShowNonSerializedField]
    private float _momentum = 0f;
    private RailInfo _oldRailInfo;
    private bool _updatedThisFrame = false;

    public float ObjRailPos { get => _objRailPos; set => _objRailPos = value; }

    private void Start()
    {
        _railImOn.ObjOnRail = this;
        ObjRailPos = Mathf.Clamp(ObjRailPos, 0f, _railImOn.RailLenght);
        transform.position = _railImOn._GetObjPos(ObjRailPos);
    }

    public void _UpdatePhysics()
    {
        if (_updatedThisFrame) return;
        _updatedThisFrame = true;

        RailInfo info = _railImOn._GetRailInfoAtPos(ObjRailPos);
        float acceleration = Vector3.Dot(Physics.gravity, info.direction) * _gravityFactor;

        _momentum += acceleration * Time.deltaTime;
        if (!_frictionFree)
        {
            if (_momentum > 0)
                _momentum -= _friction * Time.deltaTime;
            else if (_momentum < 0)
                _momentum += _friction * Time.deltaTime;
        }


        _momentum = Mathf.Clamp(_momentum, -_maxMomentum, _maxMomentum);
        if (Mathf.Abs(_momentum) < _stopThreshold)
            _momentum = 0f;


        //this. THIS FUCKING SHTI MOTHERTAKOFUCKER I HAT E YOU
        if (_momentum != 0 && Mathf.Sign(_momentum) != Mathf.Sign(acceleration) && Mathf.Abs(acceleration) > 0.001f)
        {
            _momentum = 0f;
        }

        if ((_momentum > 0 && info.direction.y > 0f) || (_momentum < 0 && info.direction.y < 0f))
        {
            _momentum = 0f;
        }



        ObjRailPos += _momentum * Time.deltaTime;
        ObjRailPos = Mathf.Clamp(ObjRailPos, 0f, _railImOn.RailLenght);

        RailInfo newInfo = _railImOn._GetRailInfoAtPos(ObjRailPos);
        transform.position = newInfo.position;

        _oldRailInfo = newInfo;
    }


    private void LateUpdate()
    {
        _UpdatePhysics();
        _updatedThisFrame = false;
    }
}
