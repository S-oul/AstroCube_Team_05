using System;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using FMODUnity;

public class ExitDoor : MonoBehaviour
{
    public bool _isDoorOpenAtStart = true;

    public static ExitDoor Instance => _instance;
    public static ExitDoor _instance;

    [SerializeField] private Vector2 _distanceAnimationStartEnd;

    [SerializeField] private GameObject _door;
    [SerializeField] private Animator _VFXAnimator;
    [SerializeField] private GameObject _stencil;
    [SerializeField] private float _endScaleStencil = 5.0f;
    [SerializeField] private Transform _zelligeDoorTransform;
    [SerializeField] private Collider _doorBlock;

    [Header("Camera Focus to end")]
    [SerializeField] private CameraFocusAttractor _cameraFocusAttractor;
    [SerializeField] private CameraFocusAttractor.CameraFocusParameters _cameraFocusParams = new(1f, 2f, .7f);

    [Header("FOV")]
    [SerializeField] private float _MaxFOV_END = 150.0f;

    [Header("Audio")]
    [SerializeField] private EventReference _zelligeDoorFXEvent;

    private Transform _playerTransform;
    private GameSettings _gameSettings;
    private bool _isShowing = false;
    private Collider _collider;

    private bool _isCurrentlyOpened;
    private float _currentLerp;
    private float _previousLerp;
    private Tween _currentTween;
    private float _lastMoveTime;
    private const float STOP_DELAY = 0.15f; // Délai avant coupure pour éviter le hachage
    private FMOD.Studio.EventInstance _doorSoundInstance;
    private bool _isDoorSoundPlaying = false;

    private void Awake()
    {
        if (_instance) Destroy(this);
        else _instance = this;

        _playerTransform = FindFirstObjectByType<PlayerMovement>().transform;

        _collider = GetComponent<Collider>();

        if (_isDoorOpenAtStart)
            OpenDoor();
        else
            CloseDoor();

        if (!_cameraFocusAttractor)
        {
            _cameraFocusAttractor = FindObjectOfType<CameraFocusAttractor>();
        }
    }

    private void Start()
    {
        _gameSettings = GameManager.Instance.Settings;
        _isShowing = false;

        //SeeExitThroughWalls();
    }

    private void OnEnable()
    {
        EventManager.OnSeeExit += SeeExitThroughWalls;
        EventManager.OnSeeExit += FocusCameraToExit;
    }

    private void OnDisable()
    {
        EventManager.OnSeeExit -= SeeExitThroughWalls;
        EventManager.OnSeeExit -= FocusCameraToExit;
    }

    [Button("Open Door")]
    public void OpenDoor()
    {
        _collider.enabled = true;

        float distance = (_zelligeDoorTransform.position - _playerTransform.position).magnitude;
        float lerp = Mathf.Clamp01(Mathf.InverseLerp(_distanceAnimationStartEnd.y, _distanceAnimationStartEnd.x, distance));
        //_VFXAnimator.SetTrigger("Open");

        _currentTween = DOTween.To(() => _currentLerp, x => _currentLerp = x, lerp, 1.5f).OnComplete(() =>
        {
            _isCurrentlyOpened = true;
        });
    }

    private void Update()
    {
        if (transform.parent.forward == -Vector3.up)
        {
            _isCurrentlyOpened = false;
            _doorBlock.enabled = true;
            _VFXAnimator.PlayInFixedTime("ZelligeDoorAnim_Open", 0, 0);

            return;
        }
        else
        {
            _doorBlock.enabled = false;
            _isCurrentlyOpened = true;
        }

        if (_isCurrentlyOpened)
        {
            float distance = (_zelligeDoorTransform.position - _playerTransform.position).magnitude;
            _currentLerp = Mathf.Clamp01(Mathf.InverseLerp(_distanceAnimationStartEnd.y, _distanceAnimationStartEnd.x, distance));
        }
        _VFXAnimator.PlayInFixedTime("ZelligeDoorAnim_Open", 0, _currentLerp);

        bool isTweenActive = _currentTween != null && _currentTween.IsActive() && _currentTween.IsPlaying();
        bool isLerpChanging = Mathf.Abs(_currentLerp - _previousLerp) > 0.0001f; // USE EPLISON GODDAMN
        bool isMoving = isTweenActive || isLerpChanging;

        if (isMoving)
        {
            _lastMoveTime = Time.time;
        }

        bool recentlyMoved = (Time.time - _lastMoveTime) < STOP_DELAY;
        bool shouldPlaySound = _currentLerp > 0.01f && recentlyMoved;

        if (shouldPlaySound && !_isDoorSoundPlaying)
        {
            _doorSoundInstance = RuntimeManager.CreateInstance(_zelligeDoorFXEvent);
            _doorSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(_zelligeDoorTransform.position));
            _doorSoundInstance.start();
            _isDoorSoundPlaying = true;
        }
        else if (!shouldPlaySound && _isDoorSoundPlaying)
        {
            _doorSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _doorSoundInstance.release();
            _isDoorSoundPlaying = false;
        }

        if (_isDoorSoundPlaying)
        {
            _doorSoundInstance.setParameterByName("DoorProgress", _currentLerp);
        }

        _previousLerp = _currentLerp;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.I))
        {
            CloseDoor();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenDoor();
        }
#endif
    }

    [Button("Close Door")]
    public void CloseDoor()
    {
        _collider.enabled = false;
        _isCurrentlyOpened = false;
        //_VFXAnimator.SetTrigger("Close");


        DOTween.To(() => _currentLerp, x => _currentLerp = x, 0.0f, 1.5f);
    }

    public void SeeExitThroughWalls()
    {
        if (_isShowing) return;
        StartCoroutine(ShowExit());
    }

    public void FocusCameraToExit()
    {

        if (_cameraFocusAttractor == null)
        {
            print(_cameraFocusAttractor.transform.name);
            return;
        }
        _cameraFocusAttractor.StopFocus();

        _cameraFocusParams.PointOfInterest = transform;
        _cameraFocusAttractor.StartFocus(_cameraFocusParams);
    }

    private IEnumerator ShowExit()
    {
        _isShowing = true;
        _stencil.SetActive(true);
        yield return _stencil.transform.DOScale(_endScaleStencil, _gameSettings.StencilFadeInDuration).WaitForCompletion();
        yield return new WaitForSeconds(_gameSettings.StencilStayDuration);
        yield return _stencil.transform.DOScale(0, _gameSettings.StencilFadeOutDuration).WaitForCompletion();
        _stencil.SetActive(false);
        _isShowing = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.parent.position, transform.parent.position + transform.parent.forward);
    }
}
