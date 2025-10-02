using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class UIManager : MonoBehaviour
{
    [SerializeField] private List<UIView> registeredViews = new List<UIView>();

    [SerializeField] private CinemachineVirtualCamera uiVirtualCamera;

    private readonly Dictionary<Type, UIView> _views = new();

    private void Awake()
    {
        foreach (var view in registeredViews)
        {
            if (view == null) continue;
            view.gameObject.SetActive(true);
            var type = view.GetType();
            if (!_views.ContainsKey(type))
                _views.Add(type, view);
            view.Hide();
            view.HideImmediate();
        }
    }

    public T GetView<T>() where T : UIView
    {
        if (_views.TryGetValue(typeof(T), out var view))
            return (T)view;

        Debug.LogError($"[UIManager] View of type {typeof(T).Name} not found.");
        return null;
    }

    public void Show<T>() where T : UIView
    {
        var view = GetView<T>();
        if (view == null) return;

        EventManager.TriggerViewShow(view);


    }



    public void Hide<T>() where T : UIView
    {
        var view = GetView<T>();
        if (view == null)
            return;
        view?.Hide();
        EventManager.TriggerViewHide(view);
    }

    public void HideAll()
    {
        foreach (var view in _views.Values)
            view.Hide();
    }

}

