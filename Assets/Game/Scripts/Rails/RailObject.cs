using NaughtyAttributes;
using System.Linq;
using UnityEngine;
using static RailConnector;

public class RailObject : MonoBehaviour
{
    [SerializeField] RailConnector _railImOn;

    float _railLenght;
    [SerializeField] float _objRailPos = 0;

    [ShowNonSerializedField] float _momemtum = 0f;
    [SerializeField] float _maxMomemtum = .5f;

    [SerializeField] float _gravityFactor = 1;

    [SerializeField, Range(0, 1), InfoBox("If 0 or 1 : it will auto snap to the Newpos")] float _lerpFactor = .1f;


    bool UpdatedThisFrame = false;
    private void Start()
    {
        _railImOn.ObjOnRail = this;
        transform.position = _railImOn._GetObjPos(ObjRailPos);
    }

    #region acccesseur
    public float ObjRailPos { get => _objRailPos; set => _objRailPos = value; }

    #endregion

    private void LateUpdate()
    {
        UpdatePhysics();
        UpdatedThisFrame = false;
    }

    public void UpdatePhysics()
    {
        if (UpdatedThisFrame) return;
        UpdatedThisFrame = true;

        RailInfo info = _railImOn._GetRailInfoAtPos(ObjRailPos);
        print("DOT : " + Vector3.Dot(info.position - transform.position, info.direction));
        if (Vector3.Dot(info.position - transform.position,info.direction) > 0)
        {
            _momemtum = 0f;
        }
        else
        {
            float acceleration = Physics.gravity.y * info.direction.y * _gravityFactor;
            _momemtum += acceleration * Time.deltaTime;
            //print(_momemtum + " // dir : " + info.direction.y);
        }



        _momemtum = Mathf.Clamp(_momemtum, -_maxMomemtum, _maxMomemtum);

        ObjRailPos += _momemtum * Time.deltaTime;
        ObjRailPos = Mathf.Clamp(ObjRailPos, 0f, _railImOn.RailLenght);

        transform.position = (_lerpFactor == 0 || _lerpFactor == 1) ? info.position : Vector3.Lerp(transform.position, info.position, _lerpFactor);

        /*if (Mathf.Abs(_momemtum) < 0.1f)
        {
            _momemtum = 0f;
        }*/
    }

    private void OnGUI()
    {
        GUILayout.Label(_momemtum.ToString());
    }
}
