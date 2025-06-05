using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPointerController : MonoBehaviour
{
    GameObject _currentlySelectedGameObject;
    Vector3 newArrowPos;

    private void Start()
    {
        newArrowPos = transform.position;
    }

    private void Update()
    {
        _currentlySelectedGameObject = EventSystem.current.currentSelectedGameObject;
        newArrowPos.y = _currentlySelectedGameObject.transform.position.y;
        transform.position = newArrowPos;
    }
}
