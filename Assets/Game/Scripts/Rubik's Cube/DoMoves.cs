using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoMoves : MonoBehaviour
{

    [SerializeField] Transform middle;

    [SerializeField] List<GameObject> Axis = new List<GameObject>();
    public List<GameObject> allBlocks = new List<GameObject>();

    public bool doScramble = true;

    private bool _isRotating = false;
    private bool _isReversing = false;

    struct RubiksMove
    {
        public Transform axis;
        public SliceAxis orientation;
        public bool clockWise;
    }

    List<RubiksMove> moves = new List<RubiksMove>();

    private void Start()
    {
        //CHANGE IF SCRIPT IS DISPLACED
        foreach (Transform t in transform.parent)
        {
            if (t.tag == "Movable") allBlocks.Add(t.gameObject);
        }

        StartCoroutine(RotateSlice(middle, true, 1f,SliceAxis.Z));

        //StartCoroutine(Scramble());
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
                RubiksMove m = CreateMove();
                RotateAngle(m, .3f);
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
                StartCoroutine(RotateAngle(moves[moves.Count - 1].axis, !moves[moves.Count - 1].clockWise, .1f));
                /*List<int> posistions = new List<int>();
                for (int i = 0; i < moves.Count - 2; i++)
                {
                    if (moves[i].axis == moves[i + 1].axis && moves[i].clockWise != moves[i + 1].clockWise)
                    {
                        posistions.Add(i);
                        posistions.Add(i++);
                    }
                }
                for (int n = posistions.Count - 1; n >= 0; n--)
                {
                    moves.RemoveAt(posistions[n]);
                }*/
                moves.RemoveAt(moves.Count - 1);
            }
            yield return null;
        }
        _isReversing = false;


    }
    public IEnumerator RotateAngle(Transform axis, bool clockWise, float duration = 0.5f)
    {
        if (_isRotating) yield return null;
        //print(axis.name + " " + clockWise);
        _isRotating = true;
        List<int> ids = new List<int>();

        foreach (var block in allBlocks)
        {
            Vector3 localBlockPos = block.transform.localPosition;
            Vector3 localAxisPos = axis.localPosition; // axis is child of the cube too

            if ((localAxisPos.x > 0.5f && localBlockPos.x > 0.5f) ||
                (localAxisPos.x < -0.5f && localBlockPos.x < -0.5f) ||
                (localAxisPos.y > 0.5f && localBlockPos.y > 0.5f) ||
                (localAxisPos.y < -0.5f && localBlockPos.y < -0.5f) ||
                (localAxisPos.z > 0.5f && localBlockPos.z > 0.5f) ||
                (localAxisPos.z < -0.5f && localBlockPos.z < -0.5f))
            {
                block.transform.SetParent(axis, true);
                ids.Add(allBlocks.IndexOf(block));
            }
        }

        int direction = clockWise ? 1 : -1;
        Vector3 rotationAxis = Vector3.zero;

        if (Mathf.Abs(axis.localPosition.x) > 0.5f)
            rotationAxis = Vector3.right;
        else if (Mathf.Abs(axis.localPosition.y) > 0.5f)
            rotationAxis = Vector3.up;
        else if (Mathf.Abs(axis.localPosition.z) > 0.5f)
            rotationAxis = Vector3.forward;

        Quaternion startRotation = axis.rotation;
        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(direction * 90, rotationAxis);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            axis.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            yield return null;
        }
        axis.rotation = targetRotation;

        foreach (int i in ids)
        {
            Vector3 pos = allBlocks[i].transform.localPosition;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            pos.z = Mathf.Round(pos.z);
            allBlocks[i].transform.localPosition = pos;
            allBlocks[i].transform.SetParent(this.transform, true);

        }
        _isRotating = false;

        if (!_isReversing)
        {
            RubiksMove move = new()
            {
                axis = axis,
                clockWise = clockWise
            };
            moves.Add(move);
        }

    }
    public enum SliceAxis { X = 0, Y, Z }

    private IEnumerator RotateSlice(Transform axis, bool clockWise, float duration, SliceAxis sliceAxis)
    {
        // Prevent overlapping rotations.
        if (_isRotating)
            yield break;

        _isRotating = true;
        List<int> ids = new List<int>();

        // Tolerance value – adjust based on the size of your cube.
        float tolerance = 0.2f;

        // Loop through each block and only select blocks that belong to the target slice.
        foreach (var block in allBlocks)
        {
            bool selectBlock = false;
            Vector3 localBlockPos = block.transform.localPosition;

            // Check only the coordinate corresponding to the desired slice.
            switch (sliceAxis)
            {
                case SliceAxis.X:
                    if (Mathf.Abs(localBlockPos.x) < tolerance)
                        selectBlock = true;
                    break;
                case SliceAxis.Y:
                    if (Mathf.Abs(localBlockPos.y) < tolerance)
                        selectBlock = true;
                    break;
                case SliceAxis.Z:
                    if (Mathf.Abs(localBlockPos.z) < tolerance)
                        selectBlock = true;
                    break;
            }

            if (selectBlock)
            {
                // Parent the block to the axis so it rotates with it.
                block.transform.SetParent(axis, true);
                ids.Add(allBlocks.IndexOf(block));
            }
        }

        // Determine the rotation axis based solely on the slice axis.
        Vector3 rotationAxis = Vector3.zero;
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

        int direction = clockWise ? 1 : -1;
        Quaternion startRotation = axis.rotation;
        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(direction * 90f, rotationAxis);

        // Animate the rotation over the specified duration.
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            axis.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            yield return null;
        }
        axis.rotation = targetRotation;

        // Reassign rotated blocks back to the cube and round their positions.
        foreach (int i in ids)
        {
            Vector3 pos = allBlocks[i].transform.localPosition;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            pos.z = Mathf.Round(pos.z);
            allBlocks[i].transform.localPosition = pos;
            allBlocks[i].transform.SetParent(this.transform, true);
        }

        _isRotating = false;

        // Optionally, record the move if you're tracking moves.
        if (!_isReversing)
        {
            RubiksMove move = new RubiksMove()
            {
                axis = axis,
                clockWise = clockWise
            };
            moves.Add(move);
        }
        StartCoroutine(RotateSlice(middle, true, 1f, (SliceAxis)Random.Range(0, 2)));

    }



    void RotateAngle(RubiksMove move, float duration = 0.5f)
    {
        StartCoroutine(RotateSlice(move.axis, move.clockWise, duration,move.orientation));
    }
    RubiksMove CreateMove()
    {
        RubiksMove move = new()
        {
            axis = Axis[Random.Range(0, Axis.Count - 1)].transform,
            orientation = (SliceAxis)Random.Range(0, 2),
            clockWise = Random.Range(0, 5) % 2 == 0
        };

        return move;
    }

    public List<GameObject> GetAxisCubes(Transform axis)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (var block in allBlocks)
        {
            Vector3 localBlockPos = block.transform.localPosition;
            Vector3 localAxisPos = axis.localPosition; // axis is child of the cube too

            if ((localAxisPos.x > 0.5f && localBlockPos.x > 0.5f) ||
                (localAxisPos.x < -0.5f && localBlockPos.x < -0.5f) ||
                (localAxisPos.y > 0.5f && localBlockPos.y > 0.5f) ||
                (localAxisPos.y < -0.5f && localBlockPos.y < -0.5f) ||
                (localAxisPos.z > 0.5f && localBlockPos.z > 0.5f) ||
                (localAxisPos.z < -0.5f && localBlockPos.z < -0.5f))
            {
                result.Add(block);
            }
        }
        return result;
    }
}
