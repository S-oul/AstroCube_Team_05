using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StarFXMovement : MonoBehaviour
{
    [SerializeField] private float _speedRotation = 20;  
    [SerializeField] private float _speedMove = 1;
    [SerializeField] private float _moveAmount;  
    void Update()
    {
        transform.RotateAround(transform.parent.position, Camera.main.transform.position - transform.parent.position, _speedRotation * Time.deltaTime);
        transform.position += (Mathf.Sin(Time.time * _speedMove) * (transform.position - transform.parent.position).normalized) * _moveAmount;
    }
}
