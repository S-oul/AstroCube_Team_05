using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DoMoves : MonoBehaviour
{

    [SerializeField] Transform middle;

    [SerializeField] List<Transform> Axis = new List<Transform>();

    List<GameObject> allBlocks = new List<GameObject>();

    public bool doScramble = true;

    private bool _isRotating = false;
    private bool _isReversing = false;

    public enum SliceAxis { X, Y, Z, Useless }
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
        if(doScramble) StartCoroutine(Scramble());
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

        List<int> ids = new List<int>();
        foreach (var block in allBlocks)
        {
            Vector3 localBlockPos = block.transform.localPosition;
            Vector3 localAxisPos = axis.localPosition; 

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

        Quaternion startRotation = axis.rotation;
        Quaternion targetRotation = Quaternion.AngleAxis(direction * 90, rotationAxis) *startRotation;
        
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
            orientation = ran == 0 ? (SliceAxis)Random.Range(0, 3) : SliceAxis.Useless,
            clockWise = Random.Range(0, 100) % 2 == 0
        };

        return move;
    }

    void RotateAxis(RubiksMove move, float duration = 0.5f)
    {
        StartCoroutine(RotateAxis(move.axis, move.clockWise, duration,move.orientation));
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
