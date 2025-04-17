using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    //Tu l'as trop ecraser cesar ce port salut
    
    [SerializeField] float _plateformSpeed = 1;
    [SerializeField] List<Transform> _allPositions = new List<Transform>();
    
    [SerializeField] float _percentagePos = 0;

    int _numberOfPos;

    private void Start()
    {
        _numberOfPos = _allPositions.Count;
    }
    void Update()
    {
        MovePlateform();
    }

    void MovePlateform()
    {
        _percentagePos += Time.deltaTime * _plateformSpeed;
        _percentagePos %= _numberOfPos;

        int plateformIndex = Mathf.FloorToInt(_percentagePos);

        transform.position = Vector3.Lerp(_allPositions[plateformIndex].position, _allPositions[plateformIndex == _numberOfPos-1? 0 : plateformIndex + 1].position, _percentagePos%1);
        transform.rotation = Quaternion.Lerp(_allPositions[plateformIndex].rotation, _allPositions[plateformIndex == _numberOfPos - 1 ? 0 : plateformIndex + 1].rotation, _percentagePos % 1);

    }

}
