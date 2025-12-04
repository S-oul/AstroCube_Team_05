using System;
using Modules.Rendering.Outline;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class OutlineThicknessOndulation : MonoBehaviour
{
    [SerializeField] private CustomPassVolume _volume;
    private OutlinePass _pass;

    private void Awake()
    {
        _pass = _volume.customPasses[0] as OutlinePass;
    }

    private void Update()
    {
        _pass.Thickness = Mathf.Lerp(5f, 15f, Mathf.PingPong(Time.time, 1.0f));
    }
}
