using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossfadeTransitionNoKaleido : MonoBehaviour
{
    public float currentOpacity;
    Image _screen;
    Animator _transitionAnimator;

    bool _isActive = true;

    [SerializeField] bool boolExecute = false;

    private void OnEnable()
    {
        EventManager.OnPlayerWin += StartFade;
    }   
    private void OnDisable()
    {
        EventManager.OnPlayerWin -= StartFade;
    }

    public void StartFade()
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
        if(boolExecute)
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
                _isActive = false;
            }
            return;
        }
        //Debug.Log("Animation is Running");

        if (_isActive == false)
        {
            _screen.enabled = true;
            _isActive = true;
        }
    }
}
