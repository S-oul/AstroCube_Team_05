using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reseter : MonoBehaviour
{
    Pose StartPos;
    Pose PoseOnReset;
    void Awake()
    {
        StartPos = new Pose();
        transform.GetPositionAndRotation(out StartPos.position, out StartPos.rotation);

        EventManager.OnPlayerReset += OnReset;
    }


    [Button]
    void OnReset()
    {
        PoseOnReset = new Pose();
        transform.GetPositionAndRotation(out PoseOnReset.position, out PoseOnReset.rotation);
        StartCoroutine(Reset(1));
    }

    IEnumerator Reset(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(PoseOnReset.position, StartPos.position, elapsedTime / time);
            transform.rotation = Quaternion.Lerp(PoseOnReset.rotation, StartPos.rotation, elapsedTime / time);
            yield return null;
        }
        transform.position = StartPos.position;
        transform.rotation = StartPos.rotation;
    }

}
