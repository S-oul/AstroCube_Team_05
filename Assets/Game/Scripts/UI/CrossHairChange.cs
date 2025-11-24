using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RubiksStatic;

public class CrossHairChange : MonoBehaviour
{
    [SerializeField] Sprite ImageX;
    [SerializeField] Sprite ImageY;
    [SerializeField] Sprite ImageZ;

    [SerializeField] Image UiCrossHair;
    private void OnEnable()
    {
        EventManager.OnCubeSwitchAxe += OnChanged;
    }

    private void OnDisable()
    {
        EventManager.OnCubeSwitchAxe -= OnChanged;
    }
    public void OnChanged()
    {
        switch (GameManager.Instance.ActualSliceAxis)
        {
            case SliceAxis.X:
                UiCrossHair.sprite = ImageX;
                break;
            case SliceAxis.Y:
                UiCrossHair.sprite = ImageY;
                break;
            case SliceAxis.Z:
                UiCrossHair.sprite = ImageZ;
                break;

        }
    }

}
