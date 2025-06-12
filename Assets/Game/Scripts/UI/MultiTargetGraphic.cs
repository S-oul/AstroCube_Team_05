using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* 
 * This script is to allow the targetting of multiple graphic elements during button hover or select.
 */
public class MultiTargetGraphic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] Color _selectedColor;
    [SerializeField] Graphic[] _uiToColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColor(_selectedColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        foreach (Graphic g in _uiToColor)
        {
            g.color = color;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetColor(_selectedColor);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetColor(Color.white);
    }
}
