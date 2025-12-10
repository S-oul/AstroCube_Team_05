using FMOD;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class BridgeVFX : MonoBehaviour
{
    private Transform _player;
    private VisualEffect _vfx;

    public Bounds BoxExtent = new Bounds();

    void Start()
    {
        BoxExtent.center = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _vfx = GetComponent<VisualEffect>();
    }

    float oldDist;
    public bool ShouldPlaySound = false;

    void Update()
    {
        //Calculate The Distance then set it ?
        _vfx.SetVector3("PlayerPos", _player.localPosition);


        var newDist = Vector3.Distance(_player.position, transform.position);
        if (Mathf.Abs(newDist - oldDist) > float.Epsilon && BoxExtent.Contains(_player.position))
        {
            if(!ShouldPlaySound)
            {
                ShouldPlaySound = true;
                print("CACA PLAY");
                //PLAY FMOD HERE;
            }
        }
        else
        {
            if (ShouldPlaySound)
            {
                print("CACA STOP");

                ShouldPlaySound = false;
                //stop FMOD HERE;
            }

        }




        oldDist = newDist;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, BoxExtent.size);
    }
}
