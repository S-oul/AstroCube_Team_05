using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraAnimator : MonoBehaviour
{
    Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            _animator.SetTrigger("StartAnimation");
        }
    }
}
