using System;
using UnityEngine;

public class RotateCubeInEditor : MonoBehaviour
{

    public RubiksMovement rubiksMovement;

    private void Reset()
    {
        rubiksMovement = GameObject.Find("Main Rubik's Cube ").GetComponentInChildren<RubiksMovement>();
    }
}
