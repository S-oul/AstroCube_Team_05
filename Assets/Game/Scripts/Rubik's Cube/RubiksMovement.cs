using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RubiksStatic;

public class RubiksMovement : MonoBehaviour
{

    [SerializeField] Transform middle;

    [SerializeField] List<Transform> Axis = new List<Transform>();

    List<GameObject> allBlocks = new List<GameObject>();

    public bool doScramble = true;

    private bool _isRotating = false;
    private bool _isReversing = false;

    struct RubiksMove
    {
        public Transform axis;
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
            if (t.tag == "Movable") allBlocks.Add(t.gameObject);
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
        /*   List<int> posistions = new List<int>();
           for (int i = 0; i < moves.Count - 2; i++)
           {
               if (moves[i].axis != middle && moves[i].axis == moves[i + 1].axis && moves[i].clockWise != moves[i + 1].clockWise)
               {
                   posistions.Add(i);
                   posistions.Add(i++);
               }
           }
           for (int n = posistions.Count - 1; n >= 0; n--)
           {
               moves.RemoveAt(posistions[n]);
           }
           moves.RemoveAt(moves.Count - 1);*/
        yield return new WaitForSeconds(.5f);
        _isReversing = true;
        while (moves.Count > 0)
        {
            if (!_isRotating)
            {
                RubiksMove m = moves[moves.Count - 1];
                StartCoroutine(RotateAxis(m.axis, !m.clockWise, .1f, m.orientation));
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
    /// <param name="clockWise">Sens de rortation de l'axe</param>
    /// <param name="duration">frere abuse un peu</param>
    /// <param name="sliceAxis">Si ce n'est pas Middle laissez vide. il est utile que pour l'axe du Mileu "Middle" il indique autour de quelle axes X/Y/Z doit tourner la slice du cube </param>
    /// <returns></returns>
    public IEnumerator RotateAxis(Transform axis, bool clockWise, float duration = 0.5f, SliceAxis sliceAxis = SliceAxis.Useless)
    {
        if (_isRotating) yield break;
        _isRotating = true;

        bool isMiddle = axis == middle;
        Vector3 localAxisPos = axis.localPosition;

        List<int> ids = new List<int>();
        foreach (var block in allBlocks)
        {
            Vector3 localBlockPos = block.transform.localPosition;

            if (isMiddle)
            {
                float blockAxisValue = sliceAxis == SliceAxis.X ? localBlockPos.x :
                                      sliceAxis == SliceAxis.Y ? localBlockPos.y :
                                      localBlockPos.z; //si X use pos.x else si Y use pos.y else use pos.z

                if (Mathf.Abs(blockAxisValue) < 0.5f)
                {
                    block.transform.SetParent(axis, true);
                    ids.Add(allBlocks.IndexOf(block));
                }
            }
            else
            {
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
        }

        int direction = clockWise ? 1 : -1;
        Vector3 rotationAxis = Vector3.zero;

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

        if (!_isReversing)
        {
            RubiksMove move = new()
            {
                axis = axis,
                orientation = sliceAxis,
                clockWise = clockWise
            };
            moves.Add(move);
        }
    }
    RubiksMove CreateRandomMove()
    {
        int ran = Random.Range(0, Axis.Count - 1);
        RubiksMove move = new()
        {
            axis = Axis[ran].transform,
            orientation = (SliceAxis)Random.Range(0, 3),
            clockWise = Random.Range(0, 100) % 2 == 0
        };

        return move;
    }

    void RotateAxis(RubiksMove move, float duration = 0.5f)
    {
        StartCoroutine(RotateAxis(move.axis, move.clockWise, duration, move.orientation));
    }

    public List<GameObject> GetCubesFromAxis(Transform cube, SliceAxis sliceAxis)
    {
        Transform axis;
        bool isMiddle = false;

        if (cube.name.Contains("Face"))
        {
            print("hey");
            axis = middle;
            isMiddle = true;
        }

        Vector3 rotationAxis = sliceAxis == SliceAxis.X ? Vector3.right :
                                      sliceAxis == SliceAxis.Y ? Vector3.forward :
                                      Vector3.up;

        List<GameObject> result = new List<GameObject>();
        foreach (var block in allBlocks)
        {

            Vector3 localBlockPos = block.transform.localPosition;
            Vector3 localRefPos = cube.transform.localPosition;

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
              (rotationAxis == Vector3.right && Mathf.Abs(localBlockPos.y - localRefPos.y) < 0.5f) || // Rotating around X -> Match Y
              (rotationAxis == Vector3.forward && Mathf.Abs(localBlockPos.x - localRefPos.x) < 0.5f); // Rotating around Z -> Match X

                if (isOnSamePlane)
                {
                    result.Add(block);
                }
            }
        }
        return result;
    }

    public Transform GetAxisFromCube(Transform cube)
    {
        if (cube.name.Contains("Face"))
        {
            return middle;
        }

        float OldDistance = float.MaxValue;
        Transform closestAxis = null;
        foreach (Transform t in Axis)
        {
            if(t != Axis[0])
            {
                float newDistance = Vector3.Distance(t.position, cube.position);
                if (newDistance < OldDistance)
                {
                    OldDistance = newDistance;
                    closestAxis = t;
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