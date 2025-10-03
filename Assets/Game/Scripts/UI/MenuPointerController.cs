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
        // Récupère l’UI sélectionnée
        _currentlySelectedGameObject = EventSystem.current.currentSelectedGameObject;
        if (_currentlySelectedGameObject == _lastSelectedGameObject) return;

        if (_currentlySelectedGameObject != null)
        {
            // Vérifie s’il existe un pointeur alternatif
            Transform alternatePointer = FindChildWithTag(_currentlySelectedGameObject, "AlternateMenuUiPointer");
            if (alternatePointer == null)
            {
                if (_altPointerImage != null)
                {
                    // Désactive les altPointers actifs
                    _altPointerImage.enabled = false;
                    _altPointerImage = null;
                    _pointerImage.enabled = true;
                }

                // Déplace le pointeur sur l’élément
                _newArrowPos.y = _currentlySelectedGameObject.transform.position.y;
                _pointerImage.color = Color.white;
                transform.position = _newArrowPos;
            }
            else
            {
                // Active altPointer et désactive le pointeur normal
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
