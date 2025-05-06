using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubiksStatic;
using System.Linq;
using UnityEngine.ProBuilder.Shapes;
using Unity.VisualScripting;

public class RubiksCubeController : MonoBehaviour
{

    [SerializeField] GameObject _controlledCube;
    RubiksMovement _controlledScript;
    RubiksMove _lastInput = null;
    bool _isPreviewDisplayed;

    [SerializeField] RubiksMovement _previewControlledScript;


    bool _cameraPlayerReversed = false;

    [SerializeField] Transform _overlayTransform;
    bool _isCameraRotating = false;

    [SerializeField] List<GameObject> ReplicatedCube = new List<GameObject>();
    [SerializeField] SelectionCube ActualFace;

    [SerializeField] bool _ShowStripLayerToPlayer = true;

    List<RubiksMovement> _replicatedScript = new List<RubiksMovement>();

    [SerializeField] Transform _player;
    [SerializeField] DetectNewParent _detectParentForGroundRotation;

    SliceAxis _selectedSlice = 0;
    private GameSettings _gameSettings;

    bool _canPlayerMoveAxis = true;


    #region Accesseur

    public bool CameraPlayerReversed { get => _cameraPlayerReversed; set => _cameraPlayerReversed = value; }
    public bool ShowStripLayerToPlayer { get => _ShowStripLayerToPlayer; set => _ShowStripLayerToPlayer = value; }
    public RubiksMovement ControlledScript { get => _controlledScript; }

    #endregion

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _detectParentForGroundRotation = _player.GetComponentInChildren<DetectNewParent>();
        if (_controlledCube != null) _controlledScript = _controlledCube.GetComponentInChildren<RubiksMovement>();
        foreach (GameObject go in ReplicatedCube)
        {
            _replicatedScript.Add(go.GetComponentInChildren<RubiksMovement>());
        }
        _gameSettings = GameManager.Instance.Settings;
        if (GameManager.Instance.IsRubiksCubeEnabled)
            ActionSwitchLineCols(true);
        if (_previewControlledScript == null)
        {
            var previewScript = GameObject.FindAnyObjectByType<PreviewRubiksCube>();
            if (previewScript)
                _previewControlledScript = previewScript.GetComponentInChildren<RubiksMovement>();
        }
    }
    private void Start()
    {
        if (_previewControlledScript)
            HidePreview();
    }


    /* OLD
    public void SetActualCube(Transform newFace)
    {
        ShutDownFace();
        GameObject newCube = null;
        if (newFace.parent.parent && newFace.parent.parent.CompareTag("Rubiks")) newCube = newFace.parent.parent.gameObject;
        else if (newFace.parent.CompareTag("Rubiks")) newCube = newFace.parent.gameObject;

        if (newCube && newCube != controlledCube)
        {
            if ((ReplicatedCube.Contains(newCube)))
            {
                print("bahoui connard");
                return;
            }
            controlledCube = newCube;
            _controlledScript = controlledCube.GetComponentInChildren<RubiksMovement>();
        }

        if (_controlledScript == null) return;
        if (ActualFace) ActualFace.enabled = false;
        ActualFace = newFace.GetComponent<Outline>();

        IlluminateFace(_selectedSlice, SelectionCube.SelectionMode.CUBE);        

        ActualFace.OutlineColor = Color.red;
        ActualFace.OutlineWidth = 15;
        ActualFace.enabled = true;

        _controlledScript.GetAxisFromCube(ActualFace.transform, _selectedSlice);
    }
    */

    public void SetActualCube(Transform newFace)
    {
        ShutDownFace();
        if (newFace.parent.parent && newFace.parent.parent.CompareTag("Rubiks")) _controlledCube = newFace.parent.parent.gameObject;
        else if (newFace.parent.CompareTag("Rubiks")) _controlledCube = newFace.parent.gameObject;

        _controlledScript = _controlledCube.GetComponentInChildren<RubiksMovement>();

        if (_controlledScript == null) return;
        if (_controlledScript.IsRotating) return;

        if (ActualFace) ActualFace.enabled = false;
        ActualFace = newFace.GetComponent<SelectionCube>();

        if (_ShowStripLayerToPlayer && TryIlluminateFace(_selectedSlice, SelectionCube.SelectionMode.AXIS))
        {
            ActualFace.Select(SelectionCube.SelectionMode.CUBE);
            _canPlayerMoveAxis = true;
        }
        else
        {
            _canPlayerMoveAxis = false;
        }

        _controlledScript.GetAxisFromCube(ActualFace.transform, _selectedSlice);
    }

    public void ActionSwitchLineCols(bool isLeft)
    {

        _selectedSlice = (SliceAxis)(((int)_selectedSlice + (isLeft ? -1 : +1) + 3) % 3);
        switch (_selectedSlice)
        {
            case SliceAxis.X:
                _detectParentForGroundRotation.DoGroundRotation = false;
                if (_controlledScript.IsLockXAxis)
                {
                    _player.SetParent(null);
                    ActionSwitchLineCols(true);
                    return;
                }
                break;
            case SliceAxis.Y:
                _detectParentForGroundRotation.DoGroundRotation = true;
                if (_controlledScript.IsLockYAxis)
                {
                    ActionSwitchLineCols(true);
                    return;
                }
                break;
            case SliceAxis.Z:
                _detectParentForGroundRotation.DoGroundRotation = false;
                if (_controlledScript.IsLockZAxis)
                {
                    _player.SetParent(null);
                    ActionSwitchLineCols(true);
                    return;
                }
                break;
        }
        if (ActualFace) SetActualCube(ActualFace.transform);
    }

    public void ActionMakeTurn(bool clockwise)
    {
        if (_controlledScript && !_controlledScript.IsRotating && _canPlayerMoveAxis)
        {
            if (_previewControlledScript && _previewControlledScript.IsRotating)
                return;
            if (_cameraPlayerReversed)
            {
                clockwise = !clockwise;
            }

            ShutDownFace();

            if (_controlledScript == null) return;
            if (ActualFace == null) return;

            foreach (RubiksMovement cube in _replicatedScript)
            {
                if (!cube) continue;

                Transform equivalence = cube.transform.GetChild(ActualFace.transform.GetComponentIndex());
                // Get The index of the children 
                // Find the Other child at the index in other cube
                // Move it

                cube.RotateAxis(cube.GetAxisFromCube(equivalence, _selectedSlice), ActualFace.transform, clockwise, _gameSettings.RubikscCubeAxisRotationDuration, _selectedSlice);

            }

            RubiksMove input = new()
            {
                Axis = _controlledScript.GetAxisFromCube(ActualFace.transform, _selectedSlice),
                cube = ActualFace.transform,
                orientation = _selectedSlice,
                clockWise = clockwise
            };

            if (_previewControlledScript)
            {
                bool completeAction = false;

                if (_lastInput != null)
                {
                    bool isSameFace = _controlledScript.GetCubesFromFace(_lastInput.cube, _lastInput.orientation).Contains(input.cube);
                    completeAction = isSameFace && _lastInput == input;
                }

                if (_previewControlledScript && !completeAction)
                {
                    _previewControlledScript.UndoMove(0.0f);
                    HidePreview();
                    ShowPreview(_selectedSlice, SelectionCube.SelectionMode.AXIS);
                    Transform equivalence = _previewControlledScript.transform.GetComponentsInChildren<Transform>().First(t => t.GetComponentIndex() == ActualFace.transform.GetComponentIndex());
                    _previewControlledScript.RotateAxis(_previewControlledScript.GetAxisFromCube(equivalence, _selectedSlice), ActualFace.transform, clockwise, _gameSettings.PreviewRubikscCubeAxisRotationDuration, _selectedSlice);
                    _lastInput = input;
                    _isPreviewDisplayed = true;
                }

                if (completeAction)
                {
                    HidePreview();
                    _controlledScript.RotateAxis(_controlledScript.GetAxisFromCube(ActualFace.transform, _selectedSlice), ActualFace.transform, clockwise, _gameSettings.RubikscCubeAxisRotationDuration, _selectedSlice);
                    _previewControlledScript.ResetMovesHistory();
                    _lastInput = null;
                    _isPreviewDisplayed = false;
                }
            }
            else
            {
                _controlledScript.RotateAxis(_controlledScript.GetAxisFromCube(ActualFace.transform, _selectedSlice), ActualFace.transform, clockwise, _gameSettings.RubikscCubeAxisRotationDuration, _selectedSlice);

            }


        }
    }
    public void ActionRotateCubeUI(Vector2 direction)
    {
        StartCoroutine(RotateCubeUI(direction));
    }

    IEnumerator RotateCubeUI(Vector2 direction)
    {
        if (!_isCameraRotating)
        {

            _isCameraRotating = true;
            float elapsedTime = 0;
            Quaternion startRotation = _overlayTransform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.AngleAxis(-90, new Vector2(direction.y, direction.x));

            while (elapsedTime < _gameSettings.UiRubikscCubeRotationDuration)
            {
                elapsedTime += Time.deltaTime;
                _overlayTransform.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / _gameSettings.UiRubikscCubeRotationDuration);
                yield return null;
            }

            _overlayTransform.transform.rotation = targetRotation;

            _isCameraRotating = false;
        }
    }

    public bool IsPlayerOnTile(SliceAxis sliceAxis, SelectionCube.SelectionMode mode)
    {
        if (_controlledScript != null)
        {
            foreach (Transform go in _controlledScript.GetCubesFromFace(ActualFace.transform, sliceAxis))
            {
                SelectionCube selection = go.GetComponent<SelectionCube>();
                if (selection == null) continue;

                if (_detectParentForGroundRotation.OldTilePlayerPos == selection && sliceAxis != SliceAxis.Y)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /* OLD
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
    */

    /// <summary>
    /// return True if Player Can Move Axis;
    /// </summary>
    /// <param name="sliceAxis"></param>
    /// <param name="mode">Should normally never be set to Locked</param>
    /// <returns></returns>
    bool TryIlluminateFace(SliceAxis sliceAxis, SelectionCube.SelectionMode mode)
    {
        List<SelectionCube> selectionCubes = new List<SelectionCube>();
        bool isOneTileLocked = false;
        bool isPlayerOnATile = false;
        if (_controlledScript != null)
        {
            foreach (Transform go in _controlledScript.GetCubesFromFace(ActualFace.transform, sliceAxis))
            {
                SelectionCube selection = go.GetComponent<SelectionCube>();
                if (selection == null) continue;


                selectionCubes.Add(selection);
                if (selection.IsTileLocked ) isOneTileLocked = true;
                if (_detectParentForGroundRotation.OldTilePlayerPos == selection && sliceAxis != SliceAxis.Y) isPlayerOnATile = true;
            }
        }

        foreach (SelectionCube selection in selectionCubes) 
        {
            if (isOneTileLocked)
            {
                selection.Select(SelectionCube.SelectionMode.LOCKED);
            }else if (isPlayerOnATile) 
            {
                selection.Select(SelectionCube.SelectionMode.PLAYERONTILE);
            }
            else
            {
                selection.Select(mode);
            }
        } 
            

        return !(isPlayerOnATile || isOneTileLocked);
    }

    void ShutDownFace()
    {
        foreach (Transform go in _controlledCube.transform)
        {
            SelectionCube selection = go.GetComponent<SelectionCube>();
            if (selection == null) continue;
            selection.Unselect();
        }
    }

    void ShowPreview(SliceAxis sliceAxis, SelectionCube.SelectionMode mode)
    {
        if (_previewControlledScript != null)
        {
            foreach (Transform go in _previewControlledScript.GetCubesFromFace(ActualFace.transform, sliceAxis))
            {
                SelectionCube selection = go.GetComponent<SelectionCube>();
                if (selection == null) continue;
                selection.Select(SelectionCube.SelectionMode.ENABLE);
            }
        }
    }

    void HidePreview()
    {
        foreach (Transform go in _previewControlledScript.transform.parent)
        {
            SelectionCube selection = go.GetComponent<SelectionCube>();
            if (selection == null) continue;
            selection.Select(SelectionCube.SelectionMode.DISABLE);
        }
    }

}


