using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpView : UIView
{
    [Header("PopUp View Elements")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TMP_Text confirmButtonText;
    [SerializeField] private TMP_Text cancelButtonText;

    private PopUpData currentData;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);

        if (confirmButton != null)
            confirmButton.onClick.AddListener(OnConfirm);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancel);
    }

    public void ShowPopup(PopUpData popUpData)
    {
        currentData = popUpData;
        UpdateContent();
        Show();
    }

    private void UpdateContent()
    {
        if (titleText != null)
            titleText.text = currentData.title;

        if (messageText != null)
            messageText.text = currentData.message;

        if (confirmButtonText != null)
            confirmButtonText.text = currentData.confirmText;

        if (cancelButtonText != null)
            cancelButtonText.text = currentData.cancelText;

        bool showCancel = currentData.popUpType != PopUpType.Info;
        cancelButton.gameObject.SetActive(showCancel);
    }

    private void OnConfirm()
    {
        currentData?.onConfirm?.Invoke();
        Hide();
    }

    private void OnCancel()
    {
        currentData?.onCancel?.Invoke();
        Hide();
    }
}
