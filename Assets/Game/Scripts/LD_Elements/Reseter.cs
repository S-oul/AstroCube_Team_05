using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Reseter : MonoBehaviour
{
    Pose _resetPos;
    Pose _poseOnReset;

    Rigidbody _rb;

    List<Pose> _positionOnLastRotation = new();

    //CONTROLLER AND CLOSE INPUTS
    void Awake()
    {
        _resetPos = new Pose();
        transform.GetPositionAndRotation(out _resetPos.position, out _resetPos.rotation);
        TryGetComponent(out _rb);


        //need
        EventManager.OnPlayerReset += OnReset;
        EventManager.OnPlayerUndo += Undo;
        
        EventManager.OnStartCubeRotation += SavePose;

        EventManager.OnPlayerResetLose += OnReset;

    }
    private void OnDisable()
    {
        EventManager.OnPlayerResetLose -= OnReset;

        EventManager.OnPlayerReset -= OnReset;
        EventManager.OnPlayerUndo -= Undo;

        EventManager.OnStartCubeRotation -= SavePose;


    }

    public void ChangeResetFunc(Transform NewPose)
    {
        NewPose.GetPositionAndRotation(out _resetPos.position,out _resetPos.rotation);
        _positionOnLastRotation.Clear();
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
            transform.position = Vector3.Lerp(_poseOnReset.position, _resetPos.position, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(_poseOnReset.rotation, _resetPos.rotation, elapsedTime / duration);
            yield return null;
        }
        transform.position = _resetPos.position;
        transform.rotation = _resetPos.rotation;

        if (gameObject.CompareTag("Player"))
        {
            InputHandler.Instance.CanMove = true;
            GetComponent<CharacterController>().excludeLayers = 0;
        }
    }



}
