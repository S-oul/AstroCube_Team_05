using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField, Range(0, 1f)]
    private float _startDelay;
    [SerializeField, Range(0, 0.2f)]
    private float _hoverAmount = 0.1f;
    [SerializeField, Range(0, 1.0f)]
    private float _hoverSpeed = 0.7f;
    [SerializeField, Range(-2.0f, 2.0f)]
    private float _rotateSpeedX = 2;
    [SerializeField, Range(-2.0f, 2.0f)]
    private float _rotateSpeedY = 2;
    [SerializeField, Range(-2.0f, 2.0f)]
    private float _rotateSpeedZ = 2;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(_startDelay + (Time.time * _hoverSpeed)) * Time.deltaTime * _hoverAmount), transform.position.z);
        transform.Rotate(new Vector3(_rotateSpeedX * Time.deltaTime, _rotateSpeedY * Time.deltaTime, _rotateSpeedZ * Time.deltaTime));
    }
}
