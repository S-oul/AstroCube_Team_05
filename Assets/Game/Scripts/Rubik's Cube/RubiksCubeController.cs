using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RubiksCubeController : MonoBehaviour
{

    [SerializeField] GameObject LilCube;
    [SerializeField] GameObject BigCube;
    
    DoMoves _lilCubeScript;
    DoMoves _bigCubeScript;



    GameObject ActualFace;

    bool _isCubeShow = false;
    bool _isRotating = false;
    bool _faceSelected = false;

    public LayerMask _detectableLayer;
    private void Awake()
    {
        _lilCubeScript = LilCube.GetComponentInChildren<DoMoves>();
        _bigCubeScript = BigCube.GetComponentInChildren<DoMoves>();

    }
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
         if (_isCubeShow)
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
    }
    [Button("Right")]
    public void ActionRight()
    {
        if (!_faceSelected)
        {
            StartCoroutine(RotateCube(Vector3.up));
        }
    }
    [Button("Up")]
    public void ActionUp()
    {
        if (!_faceSelected)
        {
            StartCoroutine(RotateCube(Vector3.right));
        }
    }
    [Button("Down")]
    public void ActionDown()
    {
        if (!_faceSelected)
        {
            StartCoroutine(RotateCube(Vector3.left));
        }
    }

    public void ActionMakeTurn(bool clockwise)
    {
        if (!_isRotating) 
        {
            _isRotating = true;
            StartCoroutine(_lilCubeScript.RotateAngle(ActualFace.transform, clockwise, .2f));
            Transform equivalence = BigCube.transform.GetChild(0).Find(ActualFace.name);
            //print(equivalence);
            StartCoroutine(_bigCubeScript.RotateAngle(equivalence, clockwise, .2f));

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
            while (elapsedTime < .4f)
            {
                elapsedTime += Time.deltaTime;
                LilCube.transform.position = new Vector3(LilCube.transform.position.x, Mathf.Lerp(-18, -6, elapsedTime / .4f), LilCube.transform.position.z);
                yield return null;
            }
            LilCube.transform.position = new Vector3(LilCube.transform.position.x, -6, LilCube.transform.position.z);

            RaycastHit _raycastInfo;
            if (Physics.Raycast(LilCube.transform.position, -LilCube.transform.parent.forward, out _raycastInfo, 500, _detectableLayer))
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
            Quaternion startRotation = LilCube.transform.rotation;
            Quaternion targetRotation = Quaternion.AngleAxis(90, direction) * startRotation;

            while (elapsedTime < .2f)
            {
                elapsedTime += Time.deltaTime;
                LilCube.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / .2f);
                yield return null;
            }

            LilCube.transform.rotation = targetRotation;
            _isRotating = false;

            RaycastHit _raycastInfo;
            if (Physics.Raycast(LilCube.transform.position, -LilCube.transform.parent.forward, out _raycastInfo, 500, _detectableLayer))
            {
                //pas opti ça shhhhhhhhhhhh
                ShutDownFace();
                ActualFace = _raycastInfo.transform.gameObject;
                IlluminateFace();
            }
        }
    }

    void IlluminateFace()
    {
        foreach (GameObject go in _lilCubeScript.GetAxisCubes(ActualFace.transform))
        {
            go.GetComponent<Outline>().enabled = true;
        }
    }
    void ShutDownFace()
    {
        foreach (GameObject go in _lilCubeScript.GetAxisCubes(ActualFace.transform))
        {
            go.GetComponent<Outline>().enabled = false;
        }
    }

}


