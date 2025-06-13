using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPointerController : MonoBehaviour
{
    GameObject _currentlySelectedGameObject;
    GameObject _lastSelectedGameObject;
    Vector3 _newArrowPos;
    Image _pointerImage;
    Image _altPointerImage;

    private void Start()
    {
        _newArrowPos = transform.position;
        _pointerImage = GetComponent<Image>();
    }

    private void Update()
    {
        // get selected UI
        _currentlySelectedGameObject = EventSystem.current.currentSelectedGameObject;
        if (_currentlySelectedGameObject == _lastSelectedGameObject ) return; // if current selected hasn't changed, end Update loop. 

        // check if selected UI has an alternate pointer
        if (_currentlySelectedGameObject != null)
        {
            Transform alternatePointer = FindChildWithTag(_currentlySelectedGameObject, "AlternateMenuUiPointer");
            if (alternatePointer == null)
            {
                if (_altPointerImage != null)
                {  // deactivate any active altPointers
                    _altPointerImage.enabled = false;
                    _altPointerImage = null;
                    _pointerImage.enabled = true;
                }

                // move pointer to correct position
                _newArrowPos.y = _currentlySelectedGameObject.transform.position.y;
                transform.position = _newArrowPos;
            }
            else
            {
                // activate altPointer and deactivate pointer
                _altPointerImage = alternatePointer.GetComponent<Image>();
                _altPointerImage.enabled = true;
                _pointerImage.enabled = false;
            }
        }

        _lastSelectedGameObject = _currentlySelectedGameObject;
    }

    Transform FindChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        return null;
    }
}
