using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelected;

    void Start()
    {
        if (InputSystemManager.Instance.CurrentInputMode == InputSystemManager.EInputMode.CONTROLLER)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelected);
        }
    }

}
