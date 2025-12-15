using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;

public class TitleScreenView : UIView
{
    [SerializeField] private EventReference _TitleScreenSound;

    private bool _waitingforInput = true;

    private void Update()
    {
        if (!_waitingforInput)
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            _waitingforInput = false;
            Hide();

            if (!_TitleScreenSound.IsNull)
            {
                RuntimeManager.PlayOneShot(_TitleScreenSound);
            }

            var uiManager = FindObjectOfType<UIManager>();
            uiManager?.Show<MainMenuView>();
        }
    }

}
