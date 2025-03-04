using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubiksStatic;
using System.Linq;

public class RubiksCubeController : MonoBehaviour
{

    [SerializeField] GameObject controlledCube;
    [SerializeField] float movementSpeed = 0.2f;
    RubiksMovement _controlledScript;
   


    [SerializeField] Transform _overlayTransform;
    [SerializeField] float _cameraSpeed = 0.2f;
    bool _isCameraRotating = false;


    [SerializeField] List<GameObject> ReplicatedCube = new List<GameObject>();


    List<RubiksMovement> _replicatedScript = new List<RubiksMovement>();


    [SerializeField] Outline ActualFace;


    SliceAxis _selectedSlice = 0;


    public LayerMask _detectableLayer;
    private void Awake()
    {
        _controlledScript = controlledCube.GetComponentInChildren<RubiksMovement>();
        foreach (GameObject go in ReplicatedCube)
        {
            _replicatedScript.Add(go.GetComponentInChildren<RubiksMovement>());
        }
    }
    public void SetActualCube(Transform newFace)
    {
        ShutDownFace();
        if (newFace.parent.parent && newFace.parent.parent.CompareTag("Rubiks")) controlledCube = newFace.parent.parent.gameObject;
        else if (newFace.parent.CompareTag("Rubiks")) controlledCube = newFace.parent.gameObject;

        _controlledScript = controlledCube.GetComponentInChildren<RubiksMovement>();
        if (_controlledScript == null) return;
        if (ActualFace) ActualFace.enabled = false;
        ActualFace = newFace.GetComponent<Outline>();

        IlluminateFace(_selectedSlice);

        ActualFace.OutlineColor = Color.red;
        ActualFace.OutlineWidth = 15;
        ActualFace.enabled = true;

        _controlledScript.GetAxisFromCube(ActualFace.transform, _selectedSlice);
    }

    public void ActionSwitchLineCols()
    {
        _selectedSlice = (SliceAxis)(((int)_selectedSlice + 1) % 3);
        SetActualCube(ActualFace.transform);
    }
    public void ActionMakeTurn(bool clockwise)
    {
        if (_controlledScript.IsRotating == false)
        {
            ShutDownFace();

            if (_controlledScript == null) return;

            foreach (RubiksMovement cube in _replicatedScript)
            {
                Transform equivalence = cube.transform.GetChild(ActualFace.transform.GetComponentIndex());
                cube.RotateAxis(cube.GetAxisFromCube(equivalence, _selectedSlice), equivalence, clockwise, movementSpeed, _selectedSlice);
            }

            _controlledScript.RotateAxis(_controlledScript.GetAxisFromCube(ActualFace.transform, _selectedSlice), ActualFace.transform, clockwise, movementSpeed, _selectedSlice);
        }
    }
    public void ActionRotateCube(Vector2 direction)
    {
        StartCoroutine(RotateCube(direction));
    }
    IEnumerator RotateCube(Vector2 direction)
    {
        if (!_isCameraRotating)
        {
            
            _isCameraRotating = true;
            float elapsedTime = 0;
            Quaternion startRotation = _overlayTransform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.AngleAxis(-90, new Vector2(direction.y,direction.x   ));

            while (elapsedTime < _cameraSpeed)
            {
                elapsedTime += Time.deltaTime;
                _overlayTransform.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / _cameraSpeed);
                yield return null;
            }

            _overlayTransform.transform.rotation = targetRotation;

            
            
            _isCameraRotating = false;
        }
    }

    void IlluminateFace(SliceAxis sliceAxis)
    {
        Color hey = new Color(1, 0.5f, 0, 1);
        if (_controlledScript != null)
            foreach (Transform go in _controlledScript.GetCubesFromFace(ActualFace.transform, sliceAxis))
            {
                Outline outline = go.GetComponent<Outline>();
                outline.OutlineColor = hey;
                outline.OutlineWidth = 10;
                outline.enabled = true;
            }
    }
    void ShutDownFace()
    {
        foreach (Transform go in controlledCube.transform)
        {
            Outline outOutline;
            if (go.TryGetComponent<Outline>(out outOutline)) outOutline.enabled = false;
        }
    }

}


