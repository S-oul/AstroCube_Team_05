using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StarMovement : MonoBehaviour
{
    public Vector3 StartPos = new();

    protected void Update()
    {
        UpdateMovement();
    }
    protected abstract void UpdateMovement();
}
