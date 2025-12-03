using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateScreenView : UIView
{

    private void Update()
    {


        if (Input.anyKeyDown)
        {
            Hide();

            var uiManager = FindObjectOfType<UIManager>();
            uiManager?.Show<MainMenuView>();
        }
    }
}
