using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateScreenView : UIView
{
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

            var uiManager = FindObjectOfType<UIManager>();
            uiManager?.Show<MainMenuView>();
        }
    }
}
