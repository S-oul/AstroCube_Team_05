using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubiksStatic;

public class RubiksCubeController : MonoBehaviour
{

    [SerializeField] GameObject ControlledCube;
    [SerializeField] List<GameObject> ReplicatedCube = new List<GameObject>();

    RubiksMovement _controlledScript;
    List<RubiksMovement> _replicatedScript = new List<RubiksMovement>();



    [SerializeField] Outline ActualFace;

    bool _isCubeShow = false;
    bool _isRotating = false;

    SliceAxis _selectedSlice = 0;


    public LayerMask _detectableLayer;
    private void Awake()
    {
        _controlledScript = ControlledCube.GetComponentInChildren<RubiksMovement>();
        foreach (GameObject go in ReplicatedCube)
        {
            _replicatedScript.Add(go.GetComponentInChildren<RubiksMovement>());
        }

    }
    public void SetActualCube(Transform newFace)
    {
        ShutDownFace();
        if (ActualFace) ActualFace.enabled = false;
        ActualFace = newFace.GetComponent<Outline>();

        IlluminateFace(_selectedSlice);        

        ActualFace.OutlineColor = Color.red;
        ActualFace.OutlineWidth = 15;
        ActualFace.enabled = true;


    }

    public void SwitchLineCols()
    {
        _selectedSlice = (SliceAxis)((int)(float)((float)_selectedSlice + 1.33f) % 3);
        print(_selectedSlice );
        SetActualCube(ActualFace.transform);
    }
    public void ActionMakeTurn(bool clockwise)
    {
        if (!_isRotating)
        {
            _isRotating = true;
            StartCoroutine(_controlledScript.RotateAxis(ActualFace.transform, clockwise, .2f,_selectedSlice));
            foreach (RubiksMovement cube in _replicatedScript)
            {
                Transform equivalence = cube.transform.GetChild(0).Find(ActualFace.name);
                StartCoroutine(cube.RotateAxis(equivalence, clockwise, .2f));
            }

            StartCoroutine(waitfor2());
        }
    }
    IEnumerator waitfor2()
    {
        yield return new WaitForSeconds(.2f);
        _isRotating = false;
    }

    void IlluminateFace(SliceAxis sliceAxis)
    {
        Color hey = new Color(1, 0.5f, 0, 1);
        foreach (GameObject go in _controlledScript.GetAxisCubes(ActualFace.transform, sliceAxis))
        {
            Outline outline = go.GetComponent<Outline>();
            outline.OutlineColor = hey;
            outline.OutlineWidth = 10;
            outline.enabled = true;
        }
    }
    void ShutDownFace()
    {
        foreach (Transform go in ControlledCube.transform)
        {
            Outline outOutline;
            if(go.TryGetComponent<Outline>(out outOutline)) outOutline.enabled = false;
        }
    }

}


