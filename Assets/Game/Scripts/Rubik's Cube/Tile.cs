using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    public UnityAction<Vector3> OnPropulsion;
    [field:SerializeField] public bool IsOccupied { get; set; }
}
