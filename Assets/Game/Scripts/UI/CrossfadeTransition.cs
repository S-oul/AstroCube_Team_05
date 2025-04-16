using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossfadeTransition : MonoBehaviour
{
    public float currentOpacity;
    public float segmentCount;
    [SerializeField] GameObject _kaleidoscopeCam;
    Image _screen;
    Animator _transitionAnimator;

    private void OnEnable()
    {
        EventManager.OnPlayerWin += StartFade;
    }   
    private void OnDisable()
    {
        EventManager.OnPlayerWin -= StartFade;
    }

    void StartFade()
    {
        _transitionAnimator.SetTrigger("StartFade");
    }

    public void ChangeSceneAfterAnimation()
    {
        EventManager.TriggerSceneChange();
    }

    private void Start()
    {
        _screen = GetComponent<Image>();
        _transitionAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //if (currentOpacity == _oldOpacity) return;
        if (!_transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("CrossfadeEnd") &&
            !_transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("CrossfadeStart"))
        {
            Debug.Log("Animation NOT running");
            if (_kaleidoscopeCam.activeSelf)
            {
                _screen.enabled = false;
                _kaleidoscopeCam.SetActive(false);
            }
            return;
        }
        Debug.Log("Animation is Running");

        if (_kaleidoscopeCam.activeSelf == false)
        {
            _screen.enabled = true;
            _kaleidoscopeCam.SetActive(true);
        }

        _screen.material.SetFloat("_Alpha", currentOpacity);
        _screen.material.SetFloat("_SegmentCount", segmentCount);
    }
}
