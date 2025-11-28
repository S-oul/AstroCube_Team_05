using System;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

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

    [Header("Camera Focus to end")]
    [SerializeField] private CameraFocusAttractor _cameraFocusAttractor;
    [SerializeField] private CameraFocusAttractor.CameraFocusParameters _cameraFocusParams = new(1f, 2f, .7f);

    [Header("FOV")]
    [SerializeField] private float _MaxFOV_END = 150.0f;

    private Transform _playerTransform;
    private GameSettings _gameSettings;
    private bool _isShowing = false;
    private Collider _collider;
    
    private bool _isCurrentlyOpened;
    private float _currentLerp;

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
        DOTween.To(() => _currentLerp, x => _currentLerp = x, lerp, 1.5f).OnComplete(() =>
        {
            _isCurrentlyOpened = true;
        });
    }

    private void Update()
    {
        if (_isCurrentlyOpened)
        {
            float distance = (_zelligeDoorTransform.position - _playerTransform.position).magnitude;
            _currentLerp = Mathf.Clamp01(Mathf.InverseLerp(_distanceAnimationStartEnd.y, _distanceAnimationStartEnd.x, distance));
        }
        _VFXAnimator.PlayInFixedTime("ZelligeDoorAnim_Open", 0, _currentLerp);
        
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
}
