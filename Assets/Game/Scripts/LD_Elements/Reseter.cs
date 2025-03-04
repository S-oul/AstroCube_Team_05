using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reseter : MonoBehaviour
{
    Pose _startPos;
    Pose _poseOnReset;

    Rigidbody _rb;
    void Awake()
    {
        _startPos = new Pose();
        transform.GetPositionAndRotation(out _startPos.position, out _startPos.rotation);
        TryGetComponent(out _rb);
        EventManager.OnPlayerReset += OnReset;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerReset -= OnReset;
    }

    [Button]
    void OnReset(float time)
    {
        if (_rb)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        _poseOnReset = new Pose();
        transform.GetPositionAndRotation(out _poseOnReset.position, out _poseOnReset.rotation);
        StartCoroutine(Reset(time));
    }

    IEnumerator Reset(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_poseOnReset.position, _startPos.position, elapsedTime / time);
            transform.rotation = Quaternion.Lerp(_poseOnReset.rotation, _startPos.rotation, elapsedTime / time);
            yield return null;
        }
        transform.position = _startPos.position;
        transform.rotation = _startPos.rotation;
    }

}
