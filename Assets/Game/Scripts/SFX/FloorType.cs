using System;
using UnityEngine;

public class FloorType : MonoBehaviour
{
    [SerializeField] private string _floorTypeTag;
    
    public string FloorTypeTag => _floorTypeTag;
    
    public bool CompareSoundTag(string tag) => _floorTypeTag == tag;

    private void Reset()
    {
        _floorTypeTag = "Default";
    }
}
