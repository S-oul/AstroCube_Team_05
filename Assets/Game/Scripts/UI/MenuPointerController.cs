using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuPointerController : MonoBehaviour
{
    private GameObject _currentlySelectedGameObject;
    private GameObject _lastSelectedGameObject;
    private Vector3 _newArrowPos;
    private Image _pointerImage;
    private Image _altPointerImage;

    private void Start()
    {
        _newArrowPos = transform.position;
        _pointerImage = GetComponent<Image>();
    }

    private void Update()
    {
        // R�cup�re l�UI s�lectionn�e
        _currentlySelectedGameObject = EventSystem.current.currentSelectedGameObject;
        if (_currentlySelectedGameObject == _lastSelectedGameObject) return;

        if (_currentlySelectedGameObject != null)
        {
            // V�rifie s�il existe un pointeur alternatif
            Transform alternatePointer = FindChildWithTag(_currentlySelectedGameObject, "AlternateMenuUiPointer");
            if (alternatePointer == null)
            {
                if (_altPointerImage != null)
                {
                    // D�sactive les altPointers actifs
                    _altPointerImage.enabled = false;
                    _altPointerImage = null;
                    _pointerImage.enabled = true;
                }

                // D�place le pointeur sur l��l�ment
                _newArrowPos.y = _currentlySelectedGameObject.transform.position.y;
                transform.position = _newArrowPos;
            }
            else
            {
                // Active altPointer et d�sactive le pointeur normal
                _altPointerImage = alternatePointer.GetComponent<Image>();
                if (_altPointerImage != null)
                {
                    _altPointerImage.enabled = true;
                    _pointerImage.enabled = false;
                }
            }
        }

        _lastSelectedGameObject = _currentlySelectedGameObject;
    }

    private Transform FindChildWithTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
                return child;
        }
        return null;
    }
}
