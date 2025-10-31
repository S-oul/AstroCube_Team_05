using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Reseter : MonoBehaviour
{
    Pose _startPos;
    Pose _poseOnReset;

    Rigidbody _rb;

    List<Pose> _positionOnLastRotation = new();

    //CONTROLLER AND CLOSE INPUTS

    void Awake()
    {
        _startPos = new Pose();
        transform.GetPositionAndRotation(out _startPos.position, out _startPos.rotation);
        TryGetComponent(out _rb);


        //need
        EventManager.OnPlayerReset += OnReset;
        EventManager.OnPlayerUndo += Undo;
        
        EventManager.OnStartCubeRotation += SavePose;

    }
    private void OnDisable()
    {
        EventManager.OnPlayerReset -= OnReset;
        EventManager.OnPlayerUndo -= Undo;

        EventManager.OnStartCubeRotation -= SavePose;


    }

    void SavePose()
    {
        if (GameManager.Instance.RubiksCube.IsReversing) return;
        
        var newPose = new Pose();
        transform.GetPositionAndRotation(out newPose.position, out newPose.rotation);
        _positionOnLastRotation.Add(newPose);

    }
    private void Undo(float time)
    {
        _poseOnReset = new Pose();
        transform.GetPositionAndRotation(out _poseOnReset.position, out _poseOnReset.rotation);
        StartCoroutine(ResetOneMove(time));
    }

    IEnumerator ResetOneMove(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_poseOnReset.position,    _positionOnLastRotation[^1].position, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(_poseOnReset.rotation, _positionOnLastRotation[^1].rotation, elapsedTime / duration);
            yield return null;
        }
        transform.position = _positionOnLastRotation[^1].position;
        transform.rotation = _positionOnLastRotation[^1].rotation;
        _positionOnLastRotation.RemoveAt(_positionOnLastRotation.Count-1);
    }
    void OnReset(float duration)
    {
        if (_rb)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        _poseOnReset = new Pose();
        transform.GetPositionAndRotation(out _poseOnReset.position, out _poseOnReset.rotation);
        StartCoroutine(Reset(duration));
        _positionOnLastRotation.Clear();
    }

    IEnumerator Reset(float duration)
    {
        if (gameObject.CompareTag("Player"))
        {
            InputHandler.Instance.CanMove = false;
            GetComponent<CharacterController>().excludeLayers = Physics.AllLayers;
        }

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_poseOnReset.position, _startPos.position, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(_poseOnReset.rotation, _startPos.rotation, elapsedTime / duration);
            yield return null;
        }
        transform.position = _startPos.position;
        transform.rotation = _startPos.rotation;

        if (gameObject.CompareTag("Player"))
        {
            InputHandler.Instance.CanMove = true;
            GetComponent<CharacterController>().excludeLayers = 0;
        }
    }

}
