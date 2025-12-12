using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;
using FMODUnity;

public class BridgeVFX : MonoBehaviour
{
    [SerializeField] private EventReference _BridgeSound;

    private FMOD.Studio.EventInstance _audioInstance;
    private Transform _player;
    private VisualEffect _vfx;
    

    public Bounds BoxExtent = new Bounds();

    void Start()
    {
        BoxExtent.center = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _vfx = GetComponent<VisualEffect>();
        
        _audioInstance = RuntimeManager.CreateInstance(_BridgeSound);
    }

    private Vector3 _lastPlayerPos;
    private float _stopTimer;
    [SerializeField] private float _stopDelay = 0.1f;
    [SerializeField] private float _maxDistanceUpdate = 50f;
    public bool ShouldPlaySound = false;

    void Update()
    {
        _vfx.SetVector3("PlayerPos", _player.localPosition);

        float moveDist = Vector3.Distance(_player.position, _lastPlayerPos);
        bool isMoving = moveDist > 0.001f;
        bool isInside = BoxExtent.Contains(_player.position);

        if (isInside && isMoving)
        {
            _stopTimer = 0f;

            if(!ShouldPlaySound)
            {
                ShouldPlaySound = true;
                _audioInstance.start();
            }
        }
        else
        {
            _stopTimer += Time.deltaTime;

            if (ShouldPlaySound && _stopTimer > _stopDelay)
            {
                ShouldPlaySound = false;
                _audioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        if (BoxExtent.SqrDistance(_player.position) < (_maxDistanceUpdate * _maxDistanceUpdate))
        {
            Vector3 targetPos = BoxExtent.ClosestPoint(_player.position);
            _audioInstance.set3DAttributes(RuntimeUtils.To3DAttributes(targetPos));
        }

        _lastPlayerPos = _player.position;
    }

    private void OnDestroy()
    {
        if (_audioInstance.isValid())
        {
            _audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _audioInstance.release();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, BoxExtent.size);
    }
}
