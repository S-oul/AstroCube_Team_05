using System;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    [SerializeField] private List<UIView> registeredViews = new List<UIView>();

    private readonly Dictionary<Type, UIView> _views = new();

    private void Awake()
    {
        foreach (var view in registeredViews)
        {
            if (view == null) continue;
            var type = view.GetType();
            if (!_views.ContainsKey(type))
                _views.Add(type, view);

            view.Hide();
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
        view?.Show();
    }

    public void Hide<T>() where T : UIView
    {
        var view = GetView<T>();
        view?.Hide();
    }

    public void HideAll()
    {
        foreach (var view in _views.Values)
            view.Hide();
    }
}

