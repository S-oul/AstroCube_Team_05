using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameSettings Settings => settings;
    [SerializeField] private GameSettings settings;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [SerializeField] string nextScene;

    public static GameManager Instance => instance;
    private static GameManager instance;


    [SerializeField] private GameObject _rubiksCubeUI;
    public bool IsRubiksCubeEnabled => _isRubiksCubeEnabled;
    [SerializeField, ReadOnly] private bool _isRubiksCubeEnabled;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }
    public void Screenshake(float duration, float amount, int vibrato)
    {
        Camera.main.DOShakePosition(duration, amount);
    }
    private void OnEnable()
    {
        EventManager.OnSceneChange += ChangeScene;

        EventManager.OnGamePause += StopDeltTime;
        EventManager.OnGamePause += UnlockMouse;
        EventManager.OnGameUnpause += ResetDeltaTime;
        EventManager.OnGameUnpause += LockMouse;
    }

    private void OnDisable()
    {
        EventManager.OnSceneChange -= ChangeScene;
        
        EventManager.OnPlayerWin -= ShowWinScreen;
        EventManager.OnPlayerLose -= ShowLoseScreen;

        EventManager.OnGamePause -= StopDeltTime;
        EventManager.OnGamePause -= UnlockMouse;
        EventManager.OnGameUnpause -= ResetDeltaTime;
        EventManager.OnGameUnpause -= LockMouse;
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

    void StopDeltTime()
    {
        Time.timeScale = 0;
    }    
    
    void ResetDeltaTime()
    {
        Time.timeScale = 1f;
    }

    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    [Button("Enable Rubik's Cube")]
    public void EnableRubiksCube() => ToggleRubiksCube(true);

    [Button("Disable Rubik's Cube")]
    public void DisableRubiksCube() => ToggleRubiksCube(false);
    
    private void ToggleRubiksCube(bool isEnabled)
    {
        _isRubiksCubeEnabled = isEnabled;
        _rubiksCubeUI.SetActive(isEnabled);
    }
}
