using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RightActionObject : MonoBehaviour
{
    public Pose RightPose => _rightPose;
    [SerializeField, ReadOnly] private Pose _rightPose;

    public Pose GetActualPose()
    {
        Pose pose = new()
        {
            position = transform.localPosition,
            rotation = transform.localRotation,
        };
        return pose;
    }

    private void Awake()
    {
        var infos = PlayModePositionSaver.PositionsSave;
        if (infos == null || infos.RightActionInfos == null)
        {
            Debug.LogWarning("You must save object in their final positions");
            return;            
        }
        else if(infos.RightActionInfos == null)
        {
            Debug.LogWarning("You must save object in their final positions");
            return;
        }
        var info = infos.RightActionInfos.FirstOrDefault(x => x.ObjectRef == gameObject);
        if(info != null)
            _rightPose = info.Pose;
    }
}
