using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameSettings Settings => settings;
    [SerializeField] private GameSettings settings;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [SerializeField][Scene] string nextScene;

    [Header("Entity Sequence")]
    [SerializeField] EntitySequenceManager _entitySequenceManager;
    [SerializeField] Image _fade;
    [SerializeField] float _sequenceDuration;
    [SerializeField] Transform _artifact;
    [SerializeField] List<GameObject> _objectToDisable;

    public static GameManager Instance => instance;
    private static GameManager instance;


    [SerializeField] private GameObject _rubiksCubeUI;
    public bool IsRubiksCubeEnabled => _isRubiksCubeEnabled;
    [SerializeField, ReadOnly] private bool _isRubiksCubeEnabled;

    [SerializeField] GameObject _previewRubiksCube; 

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    public enum EScreenshakeMode
    {
        RUBIKS_CUBE_ROTATION
    }

    public void Screenshake(EScreenshakeMode mode)
    {
        switch (mode)
        {
            default:
                break;
            case EScreenshakeMode.RUBIKS_CUBE_ROTATION:
                Camera.main.DOShakePosition(settings.RubiksCubeRotationScreenshakeSettings.x,
                                            settings.RubiksCubeRotationScreenshakeSettings.y,
                                            (int)settings.RubiksCubeRotationScreenshakeSettings.z,
                                            settings.RubiksCubeRotationScreenshakeSettings.w);
                break;
        }
    }
    void ScreenshakeCubeRotation() => Screenshake(EScreenshakeMode.RUBIKS_CUBE_ROTATION);

    private void OnEnable()
    {
        EventManager.OnSceneChange += ChangeScene;

        EventManager.OnStartCubeRotation += ScreenshakeCubeRotation;

        EventManager.OnGamePause += StopDeltaTime;
        EventManager.OnGameUnpause += ResetDeltaTime;

        EventManager.OnGamePause += UnlockMouse;
        EventManager.OnGameUnpause += LockMouse;

        EventManager.OnPreviewChange += PreviewSetActive;
    }

    private void OnDisable()
    {
        EventManager.OnSceneChange -= ChangeScene;

        EventManager.OnStartCubeRotation -= ScreenshakeCubeRotation;

        EventManager.OnGamePause -= StopDeltaTime;
        EventManager.OnGameUnpause -= ResetDeltaTime;
        EventManager.OnGameUnpause -= LockMouse;
        EventManager.OnGamePause -= UnlockMouse;

        EventManager.OnPreviewChange -= PreviewSetActive;
    }

    private void Start()
    {
        EventManager.TriggerSceneStart();
    }

    void ShowWinScreen()
    {
        winScreen.SetActive(true);
        Debug.Log("Victoire !");
    }

    void ShowLoseScreen()
    {
        loseScreen.SetActive(true);
        Debug.Log("D�faite !");
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    void StopDeltaTime()
    {
        Time.timeScale = 0;
    }    
    
    void ResetDeltaTime()
    {
        Time.timeScale = 1f;
    }

    void LockMouse()
    {
        if (InputSystemManager.Instance.CurrentInputMode == InputSystemManager.EInputMode.KEYBOARD)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void UnlockMouse()
    {
        if (InputSystemManager.Instance.CurrentInputMode == InputSystemManager.EInputMode.KEYBOARD || Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void StartSequence(Quaternion cameraAngle) => StartCoroutine(ShowSequence(cameraAngle));
    public IEnumerator ShowSequence(Quaternion cameraAngle)
    {
        EventManager.TriggerNarrativeSequenceStart();

        yield return DOTween.To(() => new Color(0, 0, 0, 0), x => _fade.color = x, new Color(0, 0, 0, 1.0f), 1.0f).WaitForCompletion();
        _fade.color = new Color(0, 0, 0, 0);

        foreach (var obj in _objectToDisable)
        {
            obj.gameObject.SetActive(false);
        }
        _artifact.gameObject.SetActive(false);
        _entitySequenceManager.gameObject.SetActive(true);

        EnableRubiksCube();
        
        yield return new WaitForSeconds(_sequenceDuration);

        _entitySequenceManager.gameObject.SetActive(false);

        foreach (var obj in _objectToDisable)
        {
            obj.gameObject.SetActive(true);
        }

        EventManager.TriggerNarrativeSequenceEnd();
        _fade.color = new Color(0, 0, 0, 1.0f);
        Camera.main.transform.parent.parent.rotation = Quaternion.Euler(0, cameraAngle.eulerAngles.y - 180, 0);
        Camera.main.transform.parent.parent.position = new Vector3(0, Camera.main.transform.parent.parent.position.y, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);

        yield return DOTween.To(() => new Color(0, 0, 0, 1.0f), x => _fade.color = x, new Color(0, 0, 0, 0.0f), 1.0f).WaitForCompletion();

        //Mouse.current.WarpCursorPosition(new Vector2(0,0));

        EventManager.TriggerActivateCubeSequence();
        EventManager.OnEndSequence += EndNarrativeSequence;
        yield return Camera.main.transform.parent.parent.DORotate(new Vector3(Camera.main.transform.parent.parent.eulerAngles.x, 359, Camera.main.transform.parent.parent.eulerAngles.z), 10, RotateMode.WorldAxisAdd).WaitForCompletion();
    }

    private void EndNarrativeSequence() => InputHandler.Instance.CanMove = true;

    [Button("Enable Rubik's Cube")]
    public void EnableRubiksCube() => ToggleRubiksCube(true);

    [Button("Disable Rubik's Cube")]
    public void DisableRubiksCube() => ToggleRubiksCube(false);
    
    private void ToggleRubiksCube(bool isEnabled)
    {
        _isRubiksCubeEnabled = isEnabled;
        _rubiksCubeUI.SetActive(isEnabled);
    }

    private void PreviewSetActive(bool isEnabled = true)
    {
        // PreviewRubiksCube functionality cannot yet be toggled during playmode. 

        /*
        if (_previewRubiksCube == null)
        {
            Debug.Log("No 'Preview Rubic's Cube' is present in this scene.");
            return;
        }
        _previewRubiksCube.SetActive(isEnabled);
        */
    }
}
