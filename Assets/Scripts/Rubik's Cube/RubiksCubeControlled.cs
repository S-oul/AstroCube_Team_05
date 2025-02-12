using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RubiksCubeControlled : MonoBehaviour
{

    [SerializeField] GameObject Lilcube;
    [SerializeField] GameObject EmptyParent;

    [SerializeField] GameObject ActualFace;

    bool _isCubeShow = false;
    bool _isRotating = false;
    bool _faceSelected = false;


    [Button("Show Cube")]
    void ActionShowUpcube()
    {
        StartCoroutine(ShowpCube());
    }
    IEnumerator ShowpCube()
    {
        float elapsedTime = 0;
        while (elapsedTime < .5f)
        {
            elapsedTime += Time.deltaTime;
            Lilcube.transform.position = new Vector3(0, Mathf.Lerp(0, 4, elapsedTime / .5f), 0);
            yield return null;
        }
        Lilcube.transform.position = new Vector3(0, 4, 0);
        _isCubeShow = true;
    }

    [Button("Select")]
    void ActionValidate()
    {
        Camera.main.fieldOfView -= 30;
        _faceSelected = true;
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
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward*1000, Color.red);
    }
}
