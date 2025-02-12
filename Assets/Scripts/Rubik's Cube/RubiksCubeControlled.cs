using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubiksCubeControlled : MonoBehaviour
{
    public DoMoves Rubiks;

    [SerializeField] GameObject Lilcube;
    [SerializeField] GameObject EmptyParent;

    [SerializeField] GameObject ActualFace;

    bool _isCubeShow = false;
    bool _isRotating = false;
    bool _faceSelected = false;

    public LayerMask _detectableLayer;



    [Button("Show Cube")]
    void ActionShowUpcube()
    {
        StartCoroutine(ShowpCube());
    }
    IEnumerator ShowpCube()
    {
        _isCubeShow = true;
        yield return null;
    }

    [Button("Select")]
    void ActionValidate()
    {
        if (!_faceSelected)
        {
            Camera.main.fieldOfView -= 30;
            _faceSelected = true;
        }
        else
        {
            StartCoroutine(Rubiks.RotateAngle(ActualFace.transform, true, 1f));
        }
    }

    [Button("Left")]
    void ActionLeft()
    {
        if (!_faceSelected)
        {
            StartCoroutine(RotateCube(Vector3.down));

        }
        else
        {

        }
    }
    [Button("Right")]
    void ActionRight()
    {
        if (!_faceSelected)
        {
            StartCoroutine(RotateCube(Vector3.up));
        }
        else
        {

        }
    }
    [Button("Up")]
    void ActionUp()
    {
        if (!_faceSelected)
        {
            StartCoroutine(RotateCube(Vector3.right));
        }
        else
        {

        }
    }
    [Button("Down")]
    void ActionDown()
    {
        if (!_faceSelected)
        {
            StartCoroutine(RotateCube(Vector3.left));
        }
        else
        {

        }
    }




    IEnumerator RotateCube(Vector3 direction)
    {
        if (!_isRotating)
        {
            _isRotating = true;
            float elapsedTime = 0;
            Quaternion startRotation = Lilcube.transform.rotation;
            Quaternion targetRotation = Quaternion.AngleAxis(90, direction) * startRotation;

            while (elapsedTime < .5f)
            {
                elapsedTime += Time.deltaTime;
                Lilcube.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / .5f);
                yield return null;
            }

            Lilcube.transform.rotation = targetRotation;
            _isRotating = false;

            RaycastHit _raycastInfo;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _raycastInfo, 500, _detectableLayer))
            {
                ActualFace = _raycastInfo.transform.gameObject;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 1000, Color.red);
    }
}
