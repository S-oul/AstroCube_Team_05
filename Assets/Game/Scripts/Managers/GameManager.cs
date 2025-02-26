using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    private void OnEnable()
    {
        EventManager.OnPlayerWin += ShowWinScreen;
        EventManager.OnPlayerLose += ShowLoseScreen;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerWin -= ShowWinScreen;
        EventManager.OnPlayerLose -= ShowLoseScreen;
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
