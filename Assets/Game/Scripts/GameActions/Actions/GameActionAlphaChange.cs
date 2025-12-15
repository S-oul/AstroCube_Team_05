using DG.Tweening;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionAlphaChange : AGameAction
{
    [SerializeField] List<GameObject> _targetObject = new List<GameObject>();
    List<MeshRenderer> objMat = new List<MeshRenderer>();

    [SerializeField] float timeToTransi = 3f;
    [SerializeField] bool MakeAppear = false;

    bool isFinished = false;

    Coroutine cor;



    public override string BuildGameObjectName()
    {
        return "Fade Objects";
    }
    void OnEnable()
    {
        _targetObject.ForEach(x =>
        {
            var m = x.GetComponent<MeshRenderer>();
            m.materials[0].color = new Color(0, 0, 0, MakeAppear ? 0 : 1);
            objMat.Add(m);
        }
        );
    }


    protected override void ExecuteSpecific()
    {
        if (_targetObject.Count > 0)
        {
            if (cor != null) return;
            FadeFonction();
        }
    }



    protected override bool IsFinishedSpecific()
    {
        return isFinished;
    }

    /// <summary>
    /// This function, Make Appear or disapear object base on MakeAppear, It switch it like  a flip flop
    /// </summary>
    [Button]
    public void FadeFonction()
    {
        cor = StartCoroutine(FadeSwitch());
    }

    public void FadeOut()
    {
        if (cor != null) StopCoroutine(cor);
        cor = null;

        MakeAppear = false;
        objMat.ForEach(m => m.materials[0].color = new Color(0, 0, 0, 1));
        FadeFonction();
    }
    public void FadeIn()
    {
        if (cor != null) StopCoroutine(cor);
        cor = null;

        MakeAppear = true;
        objMat.ForEach(m => m.materials[0].color = new Color(0, 0, 0, 0));
        FadeFonction();
    }


    public IEnumerator FadeSwitch()
    {
        isFinished = false;
        float timeSinceStart = 0;
        float percent = 0;

        while (timeSinceStart < timeToTransi)
        {
            timeSinceStart += Time.deltaTime;
            percent = timeSinceStart / timeToTransi;

            objMat.ForEach(m => m.materials[0].color = new Color(0, 0, 0, MakeAppear ? percent : 1 - percent));

            yield return new WaitForEndOfFrame();
        }

        objMat.ForEach(m => m.materials[0].color = new Color(0, 0, 0, MakeAppear ? 1 : 0));

        isFinished = true;
        MakeAppear = !MakeAppear;
        cor = null;
    }
}
