using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossfadeTransition : MonoBehaviour
{
    public float currentOpacity;
    public float segmentCount;
    public float kaleidoscopeCamZRotation;
    public float shaderAdjustmentAmount;

    [SerializeField] GameObject _kaleidoscopeCam;
    Image _screen;
    Animator _transitionAnimator;

    //IF YOU CHANGE THESE, I WILL FORKING KILL YOU
    float VIP_Var1 = -0.1371f; // Base 0
    float VIP_Var2 = 1.93f;    // Base 1 

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
        InputHandler.Instance.CanMove = false;
    }

    public void ChangeSceneAfterAnimation()
    {
        EventManager.TriggerSceneChange();
    }

    public void EndAnim()
    {
        InputHandler.Instance.CanMove = true;
    }

    private void Start()
    {
        _screen = GetComponent<Image>();
        _transitionAnimator = GetComponent<Animator>();
        InputHandler.Instance.CanMove = false;
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
                _kaleidoscopeCam.GetComponent<Animator>().enabled = true;
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
            _kaleidoscopeCam.GetComponent<Animator>().enabled = false;
            _isActive = true;
        }

        _screen.material.SetFloat("_Alpha", currentOpacity);
        _screen.material.SetFloat("_SegmentCount", segmentCount);
        _kaleidoscopeCam.transform.eulerAngles = new Vector3(
            _kaleidoscopeCam.transform.eulerAngles.x,
            _kaleidoscopeCam.transform.eulerAngles.y,
            kaleidoscopeCamZRotation
            );

        _screen.material.SetFloat("_TEST_VAR1", Mathf.Lerp(0, VIP_Var1, shaderAdjustmentAmount));
        _screen.material.SetFloat("_TEST_VAR2", Mathf.Lerp(1, VIP_Var2, shaderAdjustmentAmount));
    }
}
