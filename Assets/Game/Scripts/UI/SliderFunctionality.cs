using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SliderFunctionality : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    //Slider _slider;
    //bool _selected = false;
    //float _playerInput;
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

    //private void Start()
    //{
    //    _slider = GetComponentInChildren<Slider>();
    //}

    //private void Update()
    //{
    //    if (_selected == false) return;

    //    _playerInput = Input.GetAxis("Horizontal"); 
    //    if (_playerInput == 0 ) return;

    //    float _currentSliderValue = _slider.value;
    //    _slider.value = _currentSliderValue + _playerInput;
    //}

    //public void OnClick()
    //{
    //    _selected = !_selected;

    //    if (_selected)
    //    {
    //        foreach (Image image in _uiToColor) image.color = _selectedColor;
    //    } 
    //    else
    //    {
    //        foreach (Image image in _uiToColor) image.color = Color.white;
    //    }
    //}
}
