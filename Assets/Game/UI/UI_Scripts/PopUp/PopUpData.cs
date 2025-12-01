using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class PopUpData
{
    public string title;
    public string message;
    public string confirmText = "OK";
    public string cancelText = "Cancel";
    public UnityAction onConfirm;
    public UnityAction onCancel;
    public PopUpType popUpType = PopUpType.Info;

    public PopUpData(string title, string message, PopUpType type = PopUpType.Info, string confirm = "OK", string cancel = "Cancel",UnityAction onConfirm = null, UnityAction onCancel = null) 
    { 
        this.title = title;
        this.message = message;
        this.popUpType = type;
        this.confirmText = confirm;
        this.cancelText = cancel;
        this.onConfirm = onConfirm;
        this.onCancel = onCancel;
    }


}
