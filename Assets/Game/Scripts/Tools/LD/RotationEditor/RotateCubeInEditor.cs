using System;
using UnityEngine;

public class RotateCubeInEditor : MonoBehaviour
{

    private RubiksMovement _rubiksMovement;

    private void Reset()
    {
        _rubiksMovement = FindAnyObjectByType<RubiksMovement>();
    }
    
    //AXIS 
}
