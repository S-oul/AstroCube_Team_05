using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.PlasticSCM.Editor.WebApi;

public class PopUpView : UIView
{
    [Header("PopUp View Elements")]
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text messageText;
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;
    [SerializeField] TMP_Text confirmButtonText;
    [SerializeField] TMP_Text cancelButtonText;

    private PopUpData currentData;

    private void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
    }

    public void ShowPopup(PopUpData popUpData)
    {
        currentData = popUpData;
        UpdateContent();
        Show();

        Time.timeScale = 0f;
    }

    public override void Show()
    {
        base.Show();
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void UpdateContent()
    {
        titleText.text = currentData.title;
        messageText.text = currentData.message;
        confirmButtonText.text = currentData.confirmText;
        cancelButtonText.text = currentData.cancelText;
        // !!!!!!!!!!!! penser a rajouter type de popup
        cancelButton.gameObject.SetActive(currentData.popUpType == PopUpType.Info || currentData.popUpType == PopUpType.LevelConfirmationPopUp || currentData.popUpType == PopUpType.SaveErasePopUp);
    }

    private void OnConfirm()
    {
        currentData.onConfirm?.Invoke();
        Hide();
    }

    private void OnCancel()
    {
        currentData.onCancel?.Invoke();
        Hide();
    }

}
