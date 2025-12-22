using DG.Tweening;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameActionUIImageFade : AGameAction
{
    [SerializeField] List<Image> _targetObject = new List<Image>();

    [SerializeField] float _transitionTime = 3f;
    [SerializeField, UnityEngine.Range(0f, 1f)] float _alphaGoal = 3f;

    bool isFinished = false;

    public override string BuildGameObjectName()
    {
        return $"FADE IMAGES ({_targetObject.Count})";
    }

    protected override void ExecuteSpecific()
    {
        if (_targetObject.Count > 0)
        {
            foreach (Image i in _targetObject)
            {
                i.DOFade(_alphaGoal, _transitionTime).OnComplete(() => isFinished = true);
            }
        }
    }
    
    protected override bool IsFinishedSpecific()
    {
        return isFinished;
    }
}
