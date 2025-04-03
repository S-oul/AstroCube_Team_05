using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Timeline.TimelineAsset;

public class EntitySequenceManager : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Material _distortMat;
    [SerializeField] private UniversalRendererData _data;
    FullScreenPassRendererFeature _distortionRenderFeature;

    // Start is called before the first frame update
    void Start()
    {
        _text.material.SetFloat("_Alpha", 0f);
        _text.material.SetFloat("_Distort", 1f);

        _distortionRenderFeature =  _data.rendererFeatures.First(x => x.name == "Distort") as FullScreenPassRendererFeature;
        _distortionRenderFeature.passMaterial.SetFloat("_DistortionAmount", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText(string text)
    {
        _text.text = text;
        StartCoroutine(StartDisplayText(2f, 2f, 5f));
    }

    IEnumerator StartDisplayText(float durationFade, float durationDistort, float durationDisplay)
    {
        DOTween.To(() => _text.material.GetFloat("_Alpha"), x => _text.material.SetFloat("_Alpha", x), 1f, durationFade);
        DOTween.To(() => _text.material.GetFloat("_Distort"), x => _text.material.SetFloat("_Distort", x), 0f, durationDistort);
        yield return new WaitForSeconds(durationDisplay);
        DOTween.To(() => _text.material.GetFloat("_Alpha"), x => _text.material.SetFloat("_Alpha", x), 0f, 3f);
    }

    public void DistortScreen(float duration)
    {
        DOTween.To(() => _distortionRenderFeature.passMaterial.GetFloat("_DistortionAmount"), x => _distortionRenderFeature.passMaterial.SetFloat("_DistortionAmount", x), 5f, duration);
    }

    private void OnDisable()
    {
        _distortionRenderFeature.passMaterial.SetFloat("_DistortionAmount", 1);
    }
}
