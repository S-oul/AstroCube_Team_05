using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameSettings Settings => settings;
    [SerializeField] private GameSettings settings;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    public static GameManager Instance => instance;
    private static GameManager instance;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        EventManager.OnPlayerWin -= ShowWinScreen;
        EventManager.OnPlayerLose -= ShowLoseScreen;
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
        Debug.Log("Défaite !");
    }
}
