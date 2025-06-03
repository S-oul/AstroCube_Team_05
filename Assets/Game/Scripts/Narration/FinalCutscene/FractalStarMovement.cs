using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalStarMovement : StarMovement
{
    //[SerializeField] private AnimationCurve _fractalCurve;
    [SerializeField] private float _speedRotation = 20;
    [SerializeField] private float _speedMovement = 20;
    [SerializeField] private float _moveAmount = 7;
    float _startRota;

    private void Start()
    {
        var v = (transform.parent.position - transform.position);
        //transform.position += (v.normalized) * _moveAmount;
        _startRota = 360.0f * ((float)transform.GetSiblingIndex() / (float)transform.parent.childCount);
        transform.rotation = Quaternion.Euler(0, 0, _startRota);
        DOTween.To(() => 0, x => _moveAmount = x, _moveAmount, 1);

    }

    protected override void UpdateMovement()
    {
        float f = (Mathf.Sin(Time.time * _speedMovement) + 1f) / 2f * _moveAmount;

        transform.position = transform.parent.position + transform.up * f;
        transform.rotation = transform.parent.rotation * Quaternion.Euler(0, 0, (Time.time * _speedRotation) + _startRota);

        /*
        float move = (Time.time * _speedMovement) * 2 * Mathf.PI;
        var angle = Mathf.Deg2Rad * _startRota + move;
        transform.position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), Mathf.Sin(angle));
        */
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(StartPos, 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.parent.position, 0.3f); 

    }
}
