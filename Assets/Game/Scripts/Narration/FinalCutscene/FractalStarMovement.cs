using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalStarMovement : StarMovement
{
    [SerializeField] private AnimationCurve _fractalCurve;
    [SerializeField] private float _speedRotation = 20;
    [SerializeField] private float _speedMovement = 20;
    [SerializeField] private float _moveAmount = 7;
    float _startRota;

    private void Start()
    {
        var v = (transform.parent.position - transform.position);
        transform.position += (v.normalized) * _moveAmount;
        _startRota = 360.0f * ((float)transform.GetSiblingIndex() / (float)transform.parent.childCount);
        transform.rotation = Quaternion.Euler(0, 0, _startRota);

    }

    protected override void UpdateMovement()
    {
        /*
        var v = (transform.parent.position - transform.position).normalized;
        //transform.position += _fractalCurve.Evaluate(Mathf.PingPong(Time.time * _speedMovement , 1)) * _moveAmount * v;
        transform.RotateAround(transform.parent.position, Vector3.forward, _speedRotation * Time.deltaTime);
        Debug.Log(v.magnitude);
        transform.position += _speedMovement * v * Mathf.Sin(Time.time);
        //transform.position += _fractalCurve.Evaluate((Mathf.Sin(Time.time * _speedMovement) + 1) / 2) * _moveAmount * v;
        */

        float f = (Mathf.Sin(Time.time * _speedMovement) + 1f) / 2f * _moveAmount;

        transform.position = transform.parent.position + transform.up * f;
        transform.rotation = Quaternion.Euler(0, 0, (Time.time * _speedRotation) + _startRota);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(StartPos, 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.parent.position, 0.3f);

    }
}
