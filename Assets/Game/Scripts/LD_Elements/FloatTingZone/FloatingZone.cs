using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingZone : MonoBehaviour
{
    [SerializeField] float _GravityForce = 1;

    public float GravityForce => _GravityForce;
}
