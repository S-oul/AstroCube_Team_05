using System.Linq;
using UnityEngine;

public class RailObject : MonoBehaviour
{
    [SerializeField] RailConnector _railImOn;

    float _railLenght;
    [SerializeField] float _objRailPos = 0;

    float _momemtum = 0f;
    [SerializeField] float _defaultMomemtumAcceleration = 0.01f;
    [SerializeField] float _maxMomemtum = .5f;
    [SerializeField] float _treshHoldFurObjToFall = .1f;

    [SerializeField,Range(0,1)] float LerpFactor = .1f;
    private void Start()
    {

        _railImOn._GetRailLenght();
        transform.position = _railImOn._GetObjPos(_objRailPos);
    }
    GUIContent aa;
    Vector3[] v3;
    private void Update()
    {
        UpdatePhysics();
    }

    private void UpdatePhysics()
    {
        //[Forward, Backward, actual, nearestPoint]
        v3= _railImOn._GetObjAllPos(_objRailPos, _momemtum == 0 ? _defaultMomemtumAcceleration : _momemtum);
        if (v3.All(v => v == Vector3.zero))
        {
            Debug.LogError("No valid positions returned from _GetObjAllPos. Skipping movement.");
            return;
        }

        float difForward = v3[0].y - transform.position.y;
        float difBackward = v3[1].y - transform.position.y;

        int i = 0;

        if (difForward < 0f)
        {
            _momemtum = Mathf.Min(_maxMomemtum, _maxMomemtum + _defaultMomemtumAcceleration);
            _objRailPos += _momemtum;
            transform.position = Vector3.Lerp(transform.position, v3[0], LerpFactor); // forward
        }
        else if (difBackward < 0f)
        {
            _momemtum = Mathf.Min(_maxMomemtum, _maxMomemtum + _defaultMomemtumAcceleration);
            _objRailPos -= _momemtum;
            transform.position = Vector3.Lerp(transform.position, v3[1], LerpFactor); // Backward
        }
        else
        {
            _momemtum = 0;
            transform.position = Vector3.Lerp(transform.position, v3[3], .9f);
        }

        _objRailPos = Mathf.Clamp(_objRailPos, 0f, _railImOn._GetRailLenght());
    }

    private void OnGUI()
    {
        foreach (Vector3 v in v3)
        {
            aa = new GUIContent(v.ToString());
            GUILayout.Label(aa);
        }
    }

}
