using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DoMoves : MonoBehaviour
{

    [SerializeField] Transform middle;

    [SerializeField] List<GameObject> Axis = new List<GameObject>();
    [SerializeField] List<GameObject> allBlocks = new List<GameObject>();

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
        allBlocks = GameObject.FindGameObjectsWithTag("Movable").ToList();
        StartCoroutine(Scramble());
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            doScramble = false;
            List<int> posistions = new List<int>();
            for(int i = 0; i <moves.Count-2;i++)
            {
                if(moves[i].axis == moves[i + 1].axis && moves[i].clockWise != moves[i+1].clockWise)
                {
                    posistions.Add(i);
                    posistions.Add(i++);
                }
            }
            for(int n = posistions.Count-1; n >= 0; n--)
            {
                moves.RemoveAt(posistions[n]);
            }
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
                StartCoroutine(RotateAngle(m, .1f));
            }
            yield return null;
        }
    }

    IEnumerator ReverseAllMoves()
    {
        yield return new WaitForSeconds(.5f);
        while(moves.Count > 0)
        {
            if (!_isRotating)
            {
                StartCoroutine(RotateAngle(moves[moves.Count-1].axis, !moves[moves.Count - 1].clockWise, .1f));
                moves.RemoveAt(moves.Count-1);
            }
            yield return null;
        }

    }
    IEnumerator RotateAngle(Transform axis, bool clockWise, float duration = 0.5f)
    {
        print(axis.name + ' ' + clockWise);
        _isRotating = true;
        foreach (var block in allBlocks)
        {
            Vector3 blockPos = block.transform.position;

            if ((axis.position.x > 0.5f && blockPos.x > 0.5f) ||
                (axis.position.y > 0.5f && blockPos.y > 0.5f) ||
                (axis.position.z > 0.5f && blockPos.z > 0.5f) ||
                (axis.position.x < -0.5f && blockPos.x < -0.5f) ||
                (axis.position.y < -0.5f && blockPos.y < -0.5f) ||
                (axis.position.z < -0.5f && blockPos.z < -0.5f))
            {
                block.transform.SetParent(axis);
            }
        }

        Vector3 angles = axis.eulerAngles;
        float elapsedTime = 0;

        int direction = clockWise ? 1 : -1;
        Vector3 rotationAxis = Vector3.zero;

        if (axis.transform.position.x > 0.5 || axis.transform.position.x < -0.5)
            rotationAxis = Vector3.right;
        else if (axis.transform.position.y > 0.5 || axis.transform.position.y < -0.5)
            rotationAxis = Vector3.up;
        else if (axis.transform.position.z > 0.5 || axis.transform.position.z < -0.5)
            rotationAxis = Vector3.forward;

        Quaternion startRotation = axis.rotation;
        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(direction * 90, rotationAxis);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            axis.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            yield return null;
        }

        axis.rotation = targetRotation;

        foreach (GameObject t in allBlocks)
        {
            if (t.transform.position.x > 0) t.transform.parent = transform.parent;
        }
        _isRotating = false;
    }
    IEnumerator RotateAngle(RubiksMove move, float duration = 0.5f)
    {
        Transform axis = move.axis;
        bool clockWise = move.clockWise;
        print(axis.name + ' ' + clockWise);
        _isRotating = true;
        foreach (var block in allBlocks)
        {
            Vector3 blockPos = block.transform.position;

            if ((axis.position.x > 0.5f && blockPos.x > 0.5f) ||
                (axis.position.y > 0.5f && blockPos.y > 0.5f) ||
                (axis.position.z > 0.5f && blockPos.z > 0.5f) ||
                (axis.position.x < -0.5f && blockPos.x < -0.5f) ||
                (axis.position.y < -0.5f && blockPos.y < -0.5f) ||
                (axis.position.z < -0.5f && blockPos.z < -0.5f))
            {
                block.transform.SetParent(axis);
            }
        }

        Vector3 angles = axis.eulerAngles;
        float elapsedTime = 0;

        int direction = clockWise ? 1 : -1;
        Vector3 rotationAxis = Vector3.zero;

        if (axis.transform.position.x > 0.5 || axis.transform.position.x < -0.5)
            rotationAxis = Vector3.right;
        else if (axis.transform.position.y > 0.5 || axis.transform.position.y < -0.5)
            rotationAxis = Vector3.up;
        else if (axis.transform.position.z > 0.5 || axis.transform.position.z < -0.5)
            rotationAxis = Vector3.forward;

        Quaternion startRotation = axis.rotation;
        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(direction * 90, rotationAxis);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            axis.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            yield return null;
        }

        axis.rotation = targetRotation;

        foreach (GameObject t in allBlocks)
        {
            if (t.transform.position.x > 0) t.transform.parent = transform.parent;
        }
        _isRotating = false;
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
}
