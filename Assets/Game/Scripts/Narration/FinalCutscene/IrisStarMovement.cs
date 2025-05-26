using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IrisStarMovement : StarMovement
{
    public Vector3 Tangeant = new();  
    [SerializeField] private float _speedRotation = 20;  
    [SerializeField] private float _speedMove = 1;
    [SerializeField] private float _moveAmount;
    private float _startRotation;
    private void Start()
    {
        var v = (transform.parent.position - transform.position);
        transform.position += (v.normalized) * _moveAmount;
        Tangeant = Vector2.Perpendicular(v);
        Tangeant = Tangeant.normalized;
        _startRotation = 0.0f;
        transform.RotateAround(StartPos, Tangeant, _startRotation);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(StartPos, 0.3f);
        Gizmos.DrawLine(StartPos, StartPos + Tangeant);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.3f);

    }

    protected override void UpdateMovement()
    {
        transform.RotateAround(StartPos, Tangeant, _startRotation + _speedRotation * Time.deltaTime);
    }
}
