using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubiksStatic;
using System.Linq;

public class RubiksCubeController : MonoBehaviour
{

    [SerializeField] GameObject ControlledCube;
    [SerializeField] RubiksMovement _controlledScript;
    
    [SerializeField] List<GameObject> ReplicatedCube = new List<GameObject>();

    [SerializeField] float movementSpeed = 0.2f;

    List<RubiksMovement> _replicatedScript = new List<RubiksMovement>();



    [SerializeField] Outline ActualFace;

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
        if(newFace.parent.parent && newFace.parent.parent.CompareTag("Rubiks")) ControlledCube = newFace.parent.parent.gameObject;
        else if (newFace.parent.CompareTag("Rubiks")) ControlledCube = newFace.parent.gameObject;

        _controlledScript = ControlledCube.GetComponentInChildren<RubiksMovement>();
        if (_controlledScript == null) return;
        if (ActualFace) ActualFace.enabled = false;
        ActualFace = newFace.GetComponent<Outline>();

        IlluminateFace(_selectedSlice);        

        ActualFace.OutlineColor = Color.red;
        ActualFace.OutlineWidth = 15;
        ActualFace.enabled = true;

        _controlledScript.GetAxisFromCube(ActualFace.transform, _selectedSlice);
    }

    public void SwitchLineCols()
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
                //HAHAHAHAHAHAHA LA LIGNE EST HORRIBLE ALED JEROME J'T'EN SUPPLIE
                Transform equivalence = cube.transform.GetComponentsInChildren<Transform>().First(t => t.GetComponentIndex() == ActualFace.transform.GetComponentIndex());
                if (equivalence == null ) Debug.Break();
                cube.RotateAxis(cube.GetAxisFromCube(equivalence, _selectedSlice), ActualFace.transform, clockwise, movementSpeed, _selectedSlice);
            }
            
            _controlledScript.RotateAxis(_controlledScript.GetAxisFromCube(ActualFace.transform,_selectedSlice),ActualFace.transform, clockwise, movementSpeed,_selectedSlice);
            EventManager.TriggerCubeRotated();
        }
    }

    void IlluminateFace(SliceAxis sliceAxis)
    {
        Color hey = new Color(1, 0.5f, 0, 1);
        if(_controlledScript != null)
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
        foreach (Transform go in ControlledCube.transform)
        {
            Outline outOutline;
            if(go.TryGetComponent<Outline>(out outOutline)) outOutline.enabled = false;
        }
    }

}


