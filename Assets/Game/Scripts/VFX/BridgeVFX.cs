using UnityEngine;
using UnityEngine.VFX;

public class BridgeVFX : MonoBehaviour
{
    private GameObject _player;
    private VisualEffect _vfx;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {   
        _vfx.SetVector3("PlayerPos", _player.transform.localPosition);
    }
}
