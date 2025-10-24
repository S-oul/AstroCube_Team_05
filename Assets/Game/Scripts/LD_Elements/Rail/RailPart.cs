using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RailPart : MonoBehaviour
{
    [SerializeField] List<Transform> _allPos = new();
    LineRenderer _line;

    public RailObject OnlyRailObjectInScene;

    public int NumberOfPos { get => _allPos.Count; }


    [Button("setup")]
    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = _allPos.Count;
        _line.SetPositions(_allPos.ConvertAll(t => t.position).ToArray());
    }

    public List<Vector3> GetLinePos(bool reverseIt)
    {
        List<Vector3> result = new();
        if (reverseIt)
        {
            for(int i = _allPos.Count - 1; i > 0; i--)
            {
                result.Add(_allPos[i].position);
            }
            return result;
        }
        else
        {
            return result = _allPos.ConvertAll(t => t.position);
        }
    }
}
