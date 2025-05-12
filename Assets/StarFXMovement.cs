using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StarFXMovement : MonoBehaviour
{
    public Vector3 StartPos = new();  
    public Vector2 Tangeant = new();  
    [SerializeField] private float _speedRotation = 20;  
    [SerializeField] private float _speedMove = 1;
    [SerializeField] private float _moveAmount;
    private void Start()
    {
        //StartPos = transform.position;
        var v = (transform.parent.localPosition - transform.localPosition);
        transform.localPosition += ((transform.localPosition - transform.parent.localPosition).normalized) *_moveAmount;
        Tangeant = Vector2.Perpendicular(v);
    }
    void Update()
    {
        transform.RotateAround(StartPos +  transform.parent.position, Tangeant, _speedRotation * Time.deltaTime);

        //transform.RotateAround(transform.parent.position, Camera.main.transform.position - transform.parent.position, _speedRotation * Time.deltaTime);
        //transform.position += (Mathf.Sin(Time.time * _speedMove) * (transform.position - transform.parent.position).normalized) * _moveAmount;
    }
    private void OnDrawGizmosSelected()
    {
        // Set the color with custom alpha.
        Gizmos.color = Color.magenta; // Red with custom alpha

        // Draw the sphere.
        Gizmos.DrawSphere(StartPos + transform.parent.position, 1);

    }
}
