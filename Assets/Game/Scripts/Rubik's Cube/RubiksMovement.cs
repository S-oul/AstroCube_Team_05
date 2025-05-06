using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubiksStatic;
using System.Linq;
using NaughtyAttributes;
using System;

public class RubiksMovement : MonoBehaviour
{

    [Header("GD DONT TOUCH")]
    [SerializeField] bool _isPreview;
    [SerializeField] bool _isArtCube;
    [SerializeField] Transform middle;
    [SerializeField] Transform middleGameObject;

    [SerializeField] List<Transform> Axis = new List<Transform>();
    List<Transform> _allBlocks = new List<Transform>();

    [SerializeField] bool _doScramble = true;

    //PRIVATE THINGS
    private bool _isRotating = false;
    private bool _isReversing = false;
    List<RubiksMove> _moves = new List<RubiksMove>();

    [Header("LOCKINGS")]

    [SerializeField] bool _isLockXAxis;
    [SerializeField] bool _isLockYAxis;
    [SerializeField] bool _isLockZAxis;

    [Header("AUTO MOVES"), SerializeField]
    bool _DoAutoMoves = false;

    [ShowIf("_DoAutoMoves"), SerializeField] bool _PlayAtStart = false;
    [ShowIf("_DoAutoMoves"), SerializeField] bool _PlayOnEvent = false;

    [ShowIf("_DoAutoMoves"), SerializeField] int ExecuteSequenceXTime = 3;
    [InfoBox("Input -1 to let it run infinitly")]

    [ShowIf("_DoAutoMoves"), SerializeField] float TimeToRotate = 2f;
    [ShowIf("_DoAutoMoves"), SerializeField] float TimeBetweenMoves = .5f;
    [ShowIf("_DoAutoMoves"), SerializeField] float TimeBetweenSequence = 1f;
    [ShowIf("_DoAutoMoves"), SerializeField] List<RubiksMove> AutoMovesSequence = new List<RubiksMove>();



    #region Accessor
    public bool IsPreview { get => _isPreview; set => _isPreview = value; }
    public bool IsRotating { get => _isRotating; }
    public bool IsReversing { get => _isReversing; }
    public bool IsLockXAxis { get => _isLockXAxis; }
    public bool IsLockYAxis { get => _isLockYAxis; }
    public bool IsLockZAxis { get => _isLockZAxis; }
    internal List<RubiksMove> Moves { get => _moves; }

    #endregion

    private void Awake()
    {
        EventManager.OnPlayerReset += ReverseMoves;
        EventManager.OnPlayerResetOnce += UndoMove;

        foreach (Transform t in transform.parent)
        {
            if (t.tag == "Movable") _allBlocks.Add(t);
        }

        if (_doScramble) StartCoroutine(Scramble());
        else if (_PlayAtStart && AutoMovesSequence.Count > 0)
        {
            StartSequenceCoroutine();
        }

    }
    private void OnEnable()
    {
        if (_PlayOnEvent && AutoMovesSequence.Count > 0)
        {
            EventManager.OnActivateSequence += StartSequenceCoroutine;
        }
    }

    void OnDisable()
    {
        EventManager.OnPlayerReset -= ReverseMoves;
        EventManager.OnPlayerResetOnce -= UndoMove;
        EventManager.OnActivateSequence -= StartSequenceCoroutine;
    }

    void StartSequenceCoroutine()
    {
        _DoAutoMoves = true;
        StartCoroutine(FollowSequence());
    }
    IEnumerator FollowSequence()
    {
        int nbOfSquenceExecuted = 0;
        while (nbOfSquenceExecuted != ExecuteSequenceXTime)
        {
            int SequenceIndex = 0;
            while (true) //maybe While(SequenceIndex != AutoMovesSequence.Count-1) but true easier
            {
                if (!AutoMovesSequence[SequenceIndex].Axis)
                {
                    AutoMovesSequence[SequenceIndex].Axis = GetAxisFromCube(AutoMovesSequence[SequenceIndex].cube, AutoMovesSequence[SequenceIndex].orientation);
                }

                StartCoroutine(RotateAxisCoroutine(AutoMovesSequence[SequenceIndex].Axis, AutoMovesSequence[SequenceIndex].cube, AutoMovesSequence[SequenceIndex].clockWise, TimeToRotate, AutoMovesSequence[SequenceIndex].orientation));
                yield return new WaitForSeconds(TimeToRotate);

                SequenceIndex++;
                if (SequenceIndex == AutoMovesSequence.Count) break;

                yield return new WaitForSeconds(TimeBetweenMoves);
            }

            nbOfSquenceExecuted++;
            yield return new WaitForSeconds(TimeBetweenSequence);
        }
        EventManager.TriggerEndCubeSequence();
    }

    IEnumerator Scramble()
    {
        while (_doScramble)
        {
            if (!_isRotating)
            {
                RubiksMove m = CreateRandomMove();
                RotateAxis(m, .2f);
            }
            yield return null;
        }
    }
    void ReverseMoves(float timeToReset)
    {
        _doScramble = false;
        StartCoroutine(ReverseAllMoves(timeToReset));
    }
    IEnumerator ReverseAllMoves(float time)
    {
        while (_isRotating) yield return null;
        if(_moves.Count() != 0)
            time /= _moves.Count();
        else
            time = 0.0f;
        _isReversing = true;
        while (_moves.Count > 0)
        {
            if (!_isRotating)
            {
                RubiksMove m = _moves[_moves.Count - 1];
                StartCoroutine(RotateAxisCoroutine(m.Axis, m.cube, !m.clockWise, time, m.orientation));
                _moves.RemoveAt(_moves.Count - 1);
            }
            yield return null;
        }
        yield return new WaitForSeconds(time + .05f);
        _isReversing = false;
    }

    public void ResetMovesHistory()
    {
        _moves.Clear();
    }

    public void UndoMove(float time)
    {
        if (IsReversing) return;
        StartCoroutine(ReverseOneMoves(time));
    }
    IEnumerator ReverseOneMoves(float time)
    {
        while (_isRotating)
            yield return null;


        if (Moves.Count == 0) yield break;

        _isReversing = true;
        RubiksMove m = Moves[Moves.Count - 1];

        StartCoroutine(RotateAxisCoroutine(m.Axis, m.cube, !m.clockWise, time, m.orientation));
        Moves.RemoveAt(Moves.Count - 1);

        yield return new WaitForSeconds(time + .05f);
        _isReversing = false;
    }
    void RotateAxis(RubiksMove move, float duration = 0.5f)
    {
        StartCoroutine(RotateAxisCoroutine(move.Axis, move.cube, move.clockWise, duration, move.orientation));
    }

    /// <summary>
    /// Fonction qui Lance la coroutine qui permet de faire tourner n'importe quelle partie du cube.
    /// </summary>
    /// <param name="axis">L'un des 6 Axes X/Y/Z/-X/-Y/-Z ET le Milieu</param> 
    /// <param name="selectedCube">The cube the player is looknig at</param>
    /// <param name="clockWise">Sens de rortation de l'axe</param>
    /// <param name="duration">frere abuse un peu</param>
    /// <param name="sliceAxis">Indique autour de quelle axes X/Y/Z doit tourner la slice du cube </param>
    /// <returns></returns>
    public void RotateAxis(Transform axis, Transform selectedCube, bool clockWise, float duration = 0.5f, SliceAxis sliceAxis = SliceAxis.Useless)
    {
        StartCoroutine(RotateAxisCoroutine(axis, selectedCube, clockWise, duration, sliceAxis));
    }

    /// <summary>
    /// Fonction qui permet de faire tourner n'importe quelle partie du cube.
    /// </summary>
    /// <param name="axis">L'un des 6 Axes X/Y/Z/-X/-Y/-Z ET le Milieu</param> 
    /// <param name="selectedCube">The cube the player is looknig at</param>
    /// <param name="clockWise">Sens de rortation de l'axe</param>
    /// <param name="duration">frere abuse un peu</param>
    /// <param name="sliceAxis">Indique autour de quelle axes X/Y/Z doit tourner la slice du cube </param>
    /// <returns></returns>
    public IEnumerator RotateAxisCoroutine(Transform axis, Transform selectedCube, bool clockWise, float duration = 0.5f, SliceAxis sliceAxis = SliceAxis.Useless)
    {
        if (_isRotating)
        {
            if (_isPreview)
            {
                while (_isRotating)
                {
                    yield return null;
                }
            }
            else
                yield break;
        }
        _isRotating = true;

        if (!_isPreview || !_isArtCube)
        {
            if (!_DoAutoMoves) EventManager.TriggerStartCubeRotation();
            else EventManager.TriggerStartCubeSequenceRotation();
        }

        Vector3 rotationAxis = Vector3.zero;
        {
            if (Mathf.Abs(axis.localPosition.x) > 0.5f)
                rotationAxis = Vector3.right;
            else if (Mathf.Abs(axis.localPosition.y) > 0.5f)
                rotationAxis = Vector3.up;
            else if (Mathf.Abs(axis.localPosition.z) > 0.5f)
                rotationAxis = Vector3.forward;
        }

        bool isMiddle = true;

        Vector3 localAxisPos = axis.localPosition;
        Vector3 localRefPos = selectedCube.localPosition;

        List<int> blockIndexs = new List<int>();
        foreach (var block in _allBlocks)
        {
            Vector3 localBlockPos = block.transform.localPosition;

            bool isOnSamePlane =
                          (rotationAxis == Vector3.forward && Mathf.Abs(localBlockPos.z - localRefPos.z) < 0.5f)
                       || (rotationAxis == Vector3.up && Mathf.Abs(localBlockPos.y - localRefPos.y) < 0.5f)
                       || (rotationAxis == Vector3.right && Mathf.Abs(localBlockPos.x - localRefPos.x) < 0.5f);

            if (isOnSamePlane)
            {
                if (_isArtCube)
                {
                    block.GetComponentInChildren<ArtRubiksAnimator>().StartAnimRota();
                }

                if (block.name == "Corner") isMiddle = false;
                block.transform.SetParent(axis, true);
                blockIndexs.Add(_allBlocks.IndexOf(block));
            }
        }

        if (isMiddle) middleGameObject.parent = axis;

        /* Impulsion - SCRAPPED
       foreach (int i in blockIndexs)
       {
           if (_allBlocks[i].gameObject.name != "Middle")
           {
               var tiles = _allBlocks[i].transform.GetComponentsInChildren<Tile>().ToList();
               foreach (Tile tile in tiles)
               {
                   if (!tile.IsOccupied)
                       continue;
                   switch (sliceAxis)
                   {
                       case SliceAxis.X:
                           if (transform.localPosition.z - _allBlocks[i].transform.localPosition.z < 0 && clockWise
                               || transform.localPosition.z - _allBlocks[i].transform.localPosition.z > 0 && !clockWise)
                               tile.OnPropulsion?.Invoke(new Vector3(0, 0, transform.localPosition.z - _allBlocks[i].transform.localPosition.z).normalized);
                           break;
                       case SliceAxis.Y:
                           if (transform.localPosition.y - _allBlocks[i].transform.localPosition.y < 0 && clockWise
                               || transform.localPosition.y - _allBlocks[i].transform.localPosition.y > 0 && !clockWise)
                               tile.OnPropulsion?.Invoke(new Vector3(0, transform.localPosition.y - _allBlocks[i].transform.localPosition.y, 0).normalized);
                           break;
                       case SliceAxis.Z:
                           if (transform.localPosition.x - _allBlocks[i].transform.localPosition.x < 0 && clockWise
                               || transform.localPosition.x - _allBlocks[i].transform.localPosition.x > 0 && !clockWise)
                               tile.OnPropulsion?.Invoke(new Vector3(transform.localPosition.x - _allBlocks[i].transform.localPosition.x, 0, 0).normalized);
                           break;
                   }
               }
           }
       }
       */

        int direction = clockWise ? 1 : -1;

        Quaternion startRotation = axis.localRotation;
        Quaternion targetRotation = Quaternion.AngleAxis(direction * 90, rotationAxis) * startRotation;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percent = GameManager.Instance.Settings.AnimationSpeedCurve.Evaluate(elapsedTime/ duration);
            axis.localRotation = Quaternion.LerpUnclamped(startRotation, targetRotation, percent);
            yield return null;
        }
        
        
        axis.localRotation = targetRotation;


        foreach (int i in blockIndexs)
        {
            Vector3 pos = _allBlocks[i].transform.localPosition;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            pos.z = Mathf.Round(pos.z);
            _allBlocks[i].transform.localPosition = pos;
            _allBlocks[i].transform.SetParent(this.transform.parent, true);

        }

        if (isMiddle)
        {
            middleGameObject.parent = transform.parent;
        }

        _isRotating = false;

        if (!_isReversing)
        {
            RubiksMove move = new()
            {
                Axis = axis,
                cube = selectedCube,
                orientation = sliceAxis,
                clockWise = clockWise
            };
            _moves.Add(move);
        }
        if (!_isPreview || !_isArtCube)
        {
            if(!_DoAutoMoves) EventManager.TriggerEndCubeRotation();
            else EventManager.TriggerEndCubeSequenceRotation();
        }

    }

    RubiksMove CreateRandomMove()
    {
        int ran = UnityEngine.Random.Range(0, _allBlocks.Count - 1);
        RubiksMove move = new()
        {
            cube = _allBlocks[ran],
            orientation = (SliceAxis)(ran % 3),
            Axis = GetAxisFromCube(_allBlocks[ran], (SliceAxis)(ran % 3)),
            clockWise = UnityEngine.Random.Range(0, 2) % 2 == 0
        };

        return move;
    }


    public List<Transform> GetCubesFromFace(Transform cube, SliceAxis sliceAxis)
    {
        bool isMiddle = cube.name.Contains("Face");

        Vector3 rotationAxis = sliceAxis == SliceAxis.X ? Vector3.right :
                                      sliceAxis == SliceAxis.Y ? Vector3.forward :
                                      Vector3.up;

        List<Transform> result = new List<Transform>();
        foreach (var block in _allBlocks)
        {

            Vector3 localBlockPos = block.localPosition;
            Vector3 localRefPos = cube.localPosition;

            if (isMiddle)
            {
                float blockAxisValue = sliceAxis == SliceAxis.X ? localBlockPos.x :
                                      sliceAxis == SliceAxis.Y ? localBlockPos.y :
                                      localBlockPos.z; //si X use pos.x else si Y use pos.y else use pos.z

                float refAxisValue = sliceAxis == SliceAxis.X ? localRefPos.x :
                         sliceAxis == SliceAxis.Y ? localRefPos.y :
                         localRefPos.z;

                if (Mathf.Abs(blockAxisValue - refAxisValue) < 0.5f)
                {
                    result.Add(block);
                }
            }
            else
            {

                bool isOnSamePlane =
              (rotationAxis == Vector3.up && Mathf.Abs(localBlockPos.z - localRefPos.z) < 0.5f) || // Rotating around Y -> Match Z
              (rotationAxis == Vector3.forward && Mathf.Abs(localBlockPos.y - localRefPos.y) < 0.5f) || // Rotating around X -> Match Y
              (rotationAxis == Vector3.right && Mathf.Abs(localBlockPos.x - localRefPos.x) < 0.5f); // Rotating around Z -> Match X

                if (isOnSamePlane)
                {
                    result.Add(block);
                }
            }
        }

        if (result.Count(x => x.name.Contains("Middle")) > 1)
            result.Add(middleGameObject);

        return result;
    }

    public Transform GetAxisFromCube(Transform cube, SliceAxis sliceAxis)
    {
        if (cube.name.Contains("Face"))
        {
            return middle;
        }

        float OldDistance = float.MaxValue;
        Transform closestAxis = null;
        foreach (Transform t in Axis)
        {
            if (t != Axis[0])
            {
                if (t.name.Contains("X") && sliceAxis == SliceAxis.X
                || t.name.Contains("Y") && sliceAxis == SliceAxis.Y
                || t.name.Contains("Z") && sliceAxis == SliceAxis.Z)
                {
                    float newDistance = Vector3.Distance(t.position, cube.position);
                    if (newDistance < OldDistance)
                    {
                        OldDistance = newDistance;
                        closestAxis = t;
                    }
                }
            }
        }
        return closestAxis;
    }
    private void OnValidate()
    {
        if (_PlayAtStart && _PlayOnEvent) _PlayAtStart = false;
    }
}

namespace RubiksStatic
{
    [Serializable]
    class RubiksMove
    {
        public Transform axis;
        public Transform cube;
        public SliceAxis orientation;
        public bool clockWise;

        public Transform Axis { get => axis; set => axis = value; }

        public void Print()
        {
            Debug.Log("Axis : " + Axis + " cube : " + cube + " Orient : " + orientation + " ClockWise : " + clockWise);
        }

        public override bool Equals(object o)
        {
            return this == o as RubiksMove;
        }        
        
        public override int GetHashCode() => (axis, cube, orientation, clockWise).GetHashCode();

        public static bool operator ==(RubiksMove x, RubiksMove y)
        {
            if (x is null ^ y is null) return false;
            else if (x is null && y is null) return true;
            else
            {
                return (x.axis == y.axis &&
                        //x.cube == y.cube &&
                        x.orientation == y.orientation &&
                        x.clockWise == y.clockWise);
            }
        }
        public static bool operator !=(RubiksMove x, RubiksMove y)
        {
            return !(x == y);
        }
    }
    public enum SliceAxis { X, Y, Z, Useless }

}