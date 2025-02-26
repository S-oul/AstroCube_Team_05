using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubiksStatic;
using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;
using System.Linq;

public class RubiksMovement : MonoBehaviour
{

    [SerializeField] Transform middle;

    [SerializeField] List<Transform> Axis = new List<Transform>();

    List<Transform> allBlocks = new List<Transform>();

    public bool doScramble = true;

    private bool _isRotating = false;
    private bool _isReversing = false;

    struct RubiksMove
    {
        public Transform axis;
        public Transform cube;
        public SliceAxis orientation;
        public bool clockWise;

        public void Print()
        {
            Debug.Log("Axis : " + axis + " Orient : " + orientation + " ClockWise : " + clockWise);
        }
    }

    List<RubiksMove> moves = new List<RubiksMove>();

    private void Start()
    {
        //CHANGE IF SCRIPT IS DISPLACED
        foreach (Transform t in transform.parent)
        {
            if (t.tag == "Movable") allBlocks.Add(t);
        }
        if (doScramble) StartCoroutine(Scramble());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            doScramble = false;
            StartCoroutine(ReverseAllMoves());
        }
    }
    IEnumerator Scramble()
    {
        while (doScramble)
        {
            if (!_isRotating)
            {
                RubiksMove m = CreateRandomMove();
                RotateAxis(m, .2f);
            }
            yield return null;
        }
    }
    IEnumerator ReverseAllMoves()
    {
        yield return new WaitForSeconds(.5f);
        _isReversing = true;
        while (moves.Count > 0)
        {
            if (!_isRotating)
            {
                RubiksMove m = moves[moves.Count - 1];
                StartCoroutine(RotateAxis(m.axis,m.cube, !m.clockWise, .1f, m.orientation));
                moves.RemoveAt(moves.Count - 1);
            }
            yield return null;
        }
        _isReversing = false;


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
    public IEnumerator RotateAxis(Transform axis,Transform selectedCube, bool clockWise, float duration = 0.5f, SliceAxis sliceAxis = SliceAxis.Useless)
    {
        if (_isRotating) yield break;
        _isRotating = true;

        bool isMiddle = axis == middle;

        Vector3 rotationAxis = Vector3.zero;

        if (!_isReversing)
        {
            RubiksMove move = new()
            {
                axis = axis,
                cube = selectedCube,
                orientation = sliceAxis,
                clockWise = clockWise
            };
            moves.Add(move);
        }

        if (isMiddle)
        {
            switch (sliceAxis)
            {
                case SliceAxis.X:
                    rotationAxis = Vector3.right;
                    break;
                case SliceAxis.Y:
                    rotationAxis = Vector3.up;
                    break;
                case SliceAxis.Z:
                    rotationAxis = Vector3.forward;
                    break;
            }
        }
        else
        {
            if (Mathf.Abs(axis.localPosition.x) > 0.5f)
                rotationAxis = Vector3.right;
            else if (Mathf.Abs(axis.localPosition.y) > 0.5f)
                rotationAxis = Vector3.up;
            else if (Mathf.Abs(axis.localPosition.z) > 0.5f)
                rotationAxis = Vector3.forward;
        }


        Vector3 localAxisPos = axis.localPosition;
        Vector3 localRefPos = selectedCube.transform.localPosition;

        List<int> ids = new List<int>();
        foreach (var block in allBlocks)
        {
            Vector3 localBlockPos = block.transform.localPosition;

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
                    block.transform.SetParent(axis, true);
                    ids.Add(allBlocks.IndexOf(block));
                }
            }
            else
            {
                bool isOnSamePlane =
                              (rotationAxis == Vector3.forward && Mathf.Abs(localBlockPos.z - localRefPos.z) < 0.5f)
                           || (rotationAxis == Vector3.up && Mathf.Abs(localBlockPos.y - localRefPos.y) < 0.5f)
                           || (rotationAxis == Vector3.right   && Mathf.Abs(localBlockPos.x - localRefPos.x) < 0.5f);
                if (isOnSamePlane)
                {
                    block.transform.SetParent(axis, true);
                    ids.Add(allBlocks.IndexOf(block));
                }
            }
        }


        foreach (int i in ids)
        {
            if (allBlocks[i].gameObject.name != "Middle")
            {
                var tiles = allBlocks[i].transform.GetComponentsInChildren<Tile>().ToList();
                foreach (Tile tile in tiles)
                {
                    if (!tile.IsOccupied)
                        continue;
                    switch (sliceAxis)
                    {
                        case SliceAxis.X:
                            if (transform.localPosition.z - allBlocks[i].transform.localPosition.z < 0 && clockWise 
                                || transform.localPosition.z - allBlocks[i].transform.localPosition.z > 0 && !clockWise)
                                tile.OnPropulsion?.Invoke(new Vector3(0, 0, transform.localPosition.z - allBlocks[i].transform.localPosition.z).normalized);
                            break;
                        case SliceAxis.Y:
                            if (transform.localPosition.y - allBlocks[i].transform.localPosition.y < 0 && clockWise
                                || transform.localPosition.y - allBlocks[i].transform.localPosition.y > 0 && !clockWise)
                                tile.OnPropulsion?.Invoke(new Vector3(0, transform.localPosition.y - allBlocks[i].transform.localPosition.y, 0).normalized);
                            break;
                        case SliceAxis.Z:
                            if (transform.localPosition.x - allBlocks[i].transform.localPosition.x < 0 && clockWise
                                || transform.localPosition.x - allBlocks[i].transform.localPosition.x > 0 && !clockWise)
                                tile.OnPropulsion?.Invoke(new Vector3(transform.localPosition.x - allBlocks[i].transform.localPosition.x, 0, 0).normalized);
                            break;
                    }
                }
            }
        }

        int direction = clockWise ? 1 : -1;


        Quaternion startRotation = axis.localRotation;
        Quaternion targetRotation = Quaternion.AngleAxis(direction * 90, rotationAxis) * startRotation;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            axis.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            yield return null;
        }
        axis.localRotation = targetRotation;

        foreach (int i in ids)
        {           
            Vector3 pos = allBlocks[i].transform.localPosition;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            pos.z = Mathf.Round(pos.z);
            allBlocks[i].transform.localPosition = pos;
            allBlocks[i].transform.SetParent(this.transform.parent, true);
        }
        _isRotating = false;
    }

    RubiksMove CreateRandomMove()
    {
        int ran = Random.Range(0, allBlocks.Count- 1);
        RubiksMove move = new()
        {
            cube = allBlocks[ran],
            orientation = (SliceAxis)(ran % 3),
            axis = GetAxisFromCube(allBlocks[ran], (SliceAxis)(ran % 3)),
            clockWise = Random.Range(0, 2) % 2 == 0
        };

        return move;
    }

    void RotateAxis(RubiksMove move, float duration = 0.5f)
    {
        StartCoroutine(RotateAxis(move.axis,move.cube, move.clockWise, duration, move.orientation));
    }

    public List<Transform> GetCubesFromFace(Transform cube, SliceAxis sliceAxis)
    {
        bool isMiddle = false;

        if (cube.name.Contains("Face"))
        {
            isMiddle = true;
        }

        Vector3 rotationAxis = sliceAxis == SliceAxis.X ? Vector3.right :
                                      sliceAxis == SliceAxis.Y ? Vector3.forward :
                                      Vector3.up;

        List<Transform> result = new List<Transform>();
        foreach (var block in allBlocks)
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
}

namespace RubiksStatic
{
    public enum SliceAxis { X, Y, Z, Useless }

}