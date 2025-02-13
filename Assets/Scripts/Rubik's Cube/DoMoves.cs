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

    struct RubiksMove
    {
        public Transform axis;
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

        StartCoroutine(Scramble());
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
                moves.Add(m);
                RotateAngle(m, .3f);
            }
            yield return null;
        }
    }
    IEnumerator ReverseAllMoves()
    {
        yield return new WaitForSeconds(.5f);
        while (moves.Count > 0)
        {
            if (!_isRotating)
            {
                StartCoroutine(RotateAngle(moves[moves.Count - 1].axis, !moves[moves.Count - 1].clockWise, .1f));
                moves.RemoveAt(moves.Count - 1);
            }
            yield return null;
        }

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
    }
    void RotateAngle(RubiksMove move, float duration = 0.5f)
    {
        StartCoroutine(RotateAngle(move.axis, move.clockWise, duration));
    }
    RubiksMove CreateMove()
    {
        RubiksMove move = new()
        {
            axis = Axis[Random.Range(0, Axis.Count - 1)].transform,
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
