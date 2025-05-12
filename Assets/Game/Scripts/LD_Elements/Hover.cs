using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [Header("Delay")]
    [SerializeField, Range(0, 1f)]
    private float _startDelay;
    [SerializeField, Range(0, 1f)]
    private float _delayRandomness;

    [Header("Amount")]
    [SerializeField, Range(0, 0.2f)]
    private float _hoverAmount = 0.1f;
    [SerializeField, Range(0, 1f)]
    private float _amountRandomness;

    [Header("Speed")]
    [SerializeField, Range(0, 1.0f)]
    private float _hoverSpeed = 0.7f;
    [SerializeField, Range(0, 1f)]
    private float _speedRandomness;

    [Header("Rotation")]
    [SerializeField, Range(-2.0f, 2.0f)]
    private float _rotateSpeedX = 2;
    [SerializeField, Range(-2.0f, 2.0f)]
    private float _rotateSpeedY = 2;
    [SerializeField, Range(-2.0f, 2.0f)]
    private float _rotateSpeedZ = 2;
    [SerializeField, Range(0, 1f)]
    private float _rotateRandomness;

    private float rDelay, rAmount, rSpeed;
    private Vector3 rRotate;

    private void Start()
    {
        rDelay = Mathf.Clamp(Random.Range(_startDelay - _startDelay * _delayRandomness, _startDelay + _startDelay * _delayRandomness), 0, 1f);
        rAmount = Mathf.Clamp(Random.Range(_hoverAmount - _hoverAmount * _amountRandomness, _hoverAmount + _hoverAmount * _amountRandomness), 0, 1f);
        rSpeed = Mathf.Clamp(Random.Range(_hoverSpeed - _hoverSpeed * _speedRandomness, _hoverSpeed + _hoverSpeed * _speedRandomness), 0, 1f);

        rRotate = new Vector3
        (
            Mathf.Clamp(Random.Range(_rotateSpeedX - _rotateSpeedX * _rotateRandomness, _rotateSpeedX + _rotateRandomness), -2.0f, 2.0f),
            Mathf.Clamp(Random.Range(_rotateSpeedX - _rotateSpeedX * _rotateRandomness, _rotateSpeedY + _rotateRandomness), -2.0f, 2.0f),
            Mathf.Clamp(Random.Range(_rotateSpeedX - _rotateSpeedX * _rotateRandomness, _rotateSpeedZ + _rotateRandomness), -2.0f, 2.0f)
        );

    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(rDelay + (Time.time * rSpeed)) * Time.deltaTime * rAmount), transform.position.z);
        transform.Rotate(new Vector3(rRotate.x * Time.deltaTime, rRotate.y * Time.deltaTime, rRotate.z * Time.deltaTime));
    }
}
