using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RubiksCubeController : MonoBehaviour
{
    public DoMoves Rubiks;

    [SerializeField] GameObject Lilcube;
    [SerializeField] RectTransform ye2;
    GameObject ActualFace;

    bool _isCubeShow = false;
    bool _isRotating = false;
    bool _faceSelected = false;
    bool _rowOrColSelected = false;

    public LayerMask _detectableLayer;

    public void SetActualFace(GameObject newFace)
    {
        ActualFace = newFace;
    }

    [Button("Show Cube")]
    public void ActionShowUpcube()
    {
        RaycastHit _raycastInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _raycastInfo, 500, _detectableLayer))
        {
            ActualFace = _raycastInfo.transform.gameObject;
            IlluminateFace();

        }
        StartCoroutine(ShowCube());
    }

    [Button("Select")]
    public void ActionValidate()
    {
        if (!_faceSelected)
        {
            Camera.main.fieldOfView -= 30;
            _faceSelected = true;
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
        if (_rowOrColSelected)
        {
            ye2.gameObject.SetActive(false);
            _rowOrColSelected = false;
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
            ChooseRowOrCol(Vector2.left);
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
            ChooseRowOrCol(Vector2.right);
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
            ChooseRowOrCol(Vector2.up);
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
            ChooseRowOrCol(Vector2.down);
        }
    }

    public void ActionMakeTurn(bool clockwise)
    {
        if (!_isRotating) 
        {
            _isRotating = true;
            StartCoroutine(Rubiks.RotateAngle(ActualFace.transform, clockwise, .2f));
            StartCoroutine(waitfor2());
        }
    }
    IEnumerator waitfor2()
    {
        yield return new WaitForSeconds(.2f);
        _isRotating = false;
    }

    IEnumerator ShowCube()
    {
        if (!_isCubeShow)
        {
            _isCubeShow = true;
            
            float elapsedTime = 0;
            while (elapsedTime < .2f)
            {
                elapsedTime += Time.deltaTime;
                Lilcube.transform.position = new Vector3(0, Mathf.Lerp(-4, 0, elapsedTime / .2f), 0);
                yield return null;
            }
            Lilcube.transform.position = new Vector3(0, 0, 0);

            RaycastHit _raycastInfo;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _raycastInfo, 500, _detectableLayer))
            {
                ActualFace = _raycastInfo.transform.gameObject;
                IlluminateFace();
            }

        }
    }
    //unshow

    IEnumerator RotateCube(Vector3 direction)
    {
        if (!_isRotating)
        {
            _isRotating = true;
            float elapsedTime = 0;
            Quaternion startRotation = Lilcube.transform.rotation;
            Quaternion targetRotation = Quaternion.AngleAxis(90, direction) * startRotation;

            while (elapsedTime < .2f)
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
                //pas opti ça shhhhhhhhhhhh
                ShutDownFace();
                ActualFace = _raycastInfo.transform.gameObject;
                IlluminateFace();
            }
        }
    }

    void ChooseRowOrCol(Vector2 dir)
    {
        ye2.gameObject.SetActive(true);
        _rowOrColSelected = true;
        switch (dir.x, dir.y)
        {
            case (0, 1):
                ye2.eulerAngles = new Vector3(0, 0, 0);
                return;
            case (1, 0):
                ye2.eulerAngles = new Vector3(0, 0, 270);
                return;
            case (0, -1):
                ye2.eulerAngles = new Vector3(0, 0, 180);
                return;
            case (-1, 0):
                ye2.eulerAngles = new Vector3(0, 0, 90);
                return;
        }
    }

    void IlluminateFace()
    {
        foreach (GameObject go in Rubiks.GetAxisCubes(ActualFace.transform))
        {
            go.GetComponent<Outline>().enabled = true;
        }
    }
    void ShutDownFace()
    {
        foreach (GameObject go in Rubiks.GetAxisCubes(ActualFace.transform))
        {
            go.GetComponent<Outline>().enabled = false;
        }
    }


}


