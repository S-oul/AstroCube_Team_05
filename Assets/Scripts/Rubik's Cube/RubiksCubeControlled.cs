using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubiksCubeControlled : MonoBehaviour
{
    public DoMoves Rubiks;

    [SerializeField] GameObject Lilcube;
    [SerializeField] GameObject ActualFace;

    bool _isCubeShow = false;
    bool _isRotating = false;
    bool _faceSelected = false;

    public LayerMask _detectableLayer;



    [Button("Show Cube")]
    public void ActionShowUpcube()
    {
        RaycastHit _raycastInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _raycastInfo, 500, _detectableLayer))
        {
            ActualFace = _raycastInfo.transform.gameObject;
        }
        StartCoroutine(ShowpCube());
    }

    [Button("Select")]
    public void ActionValidate()
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
    [Button("De-Select")]
    public void ActionDeValidate()
    {
        if (_faceSelected)
        {
            Camera.main.fieldOfView += 30;
            _faceSelected = false;
        }
    }

    [Button("Left")]
    public void ActionLeft()
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
    public void ActionRight()
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
    public void ActionUp()
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
    public void ActionDown()
    {
        if (!_faceSelected)
        {
            StartCoroutine(RotateCube(Vector3.left));
        }
        else
        {

        }
    }


    IEnumerator ShowpCube()
    {
        if (!_isCubeShow)
        {
            _isCubeShow = true;

        }
        yield return null;
        /*
         *         float elapsedTime = 0;
        while (elapsedTime < .5f)
        {
            elapsedTime += Time.deltaTime;
            Lilcube.transform.position = new Vector3(0, Mathf.Lerp(0, 4, elapsedTime / .5f), 0);
            yield return null;
        }
        Lilcube.transform.position = new Vector3(0, 4, 0);

         */
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
                Lilcube.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / .2f);
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
