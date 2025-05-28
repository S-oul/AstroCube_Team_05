using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenuElements : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _titleScreen;

    private void Start()
    {
        _mainMenu.SetActive(false);
        _titleScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
        }
    }
}
