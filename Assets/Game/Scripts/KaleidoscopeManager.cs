using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaleidoscopeManager : MonoBehaviour
{
    [SerializeField] GameObject _sourceCamera;

    Image _screen;
    bool _isEnabled = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _screen = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (_isEnabled)
            {
                _sourceCamera.SetActive(false);
                _screen.enabled = false;
                _isEnabled = false;
            }
            else
            {
                _sourceCamera.SetActive(true);
                _screen.enabled = true;
                _isEnabled = true;
            }
        }
    }
}
