using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Vector3 _addedRotation = new();

    void Update()
    {
        transform.LookAt(Camera.main.transform);

        transform.rotation *= Quaternion.Euler(_addedRotation);
    }
}
