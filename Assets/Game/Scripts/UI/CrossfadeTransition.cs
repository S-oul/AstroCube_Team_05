using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossfadeTransition : MonoBehaviour
{
    public float currentOpacity;
    public float segmentCount;
    public float kaleidoscopeCamZRotation;

    [SerializeField] GameObject _kaleidoscopeCam;
    Image _screen;
    Animator _transitionAnimator;

    bool _isActive = true;

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
        if (!_transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("CrossfadeEnd") &&
            !_transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("CrossfadeStart"))
        {
            //Debug.Log("Animation NOT running");
            if (_isActive)
            {
                _screen.enabled = false;
                _kaleidoscopeCam.SetActive(false);
                _isActive = false;
            }
            return;
        }
        //Debug.Log("Animation is Running");

        if (_isActive == false)
        {
            _screen.enabled = true;
            _kaleidoscopeCam.SetActive(true);
            _isActive = true;
        }

        _screen.material.SetFloat("_Alpha", currentOpacity);
        _screen.material.SetFloat("_SegmentCount", segmentCount);
        _kaleidoscopeCam.transform.eulerAngles = new Vector3(
            _kaleidoscopeCam.transform.eulerAngles.x,
            _kaleidoscopeCam.transform.eulerAngles.y,
            kaleidoscopeCamZRotation
            );
    }
}
