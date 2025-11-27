using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressionListener : MonoBehaviour
{
    [SerializeField] private int totalLevels = 20;

    private void OnEnable()
    {
        EventManager.OnLevelFinished += HandleLevelFinished;
    }

    private void OnDisable()
    {
        EventManager.OnLevelFinished -= HandleLevelFinished;
    }

    private void HandleLevelFinished()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // éviter de save le main menu
        if (LevelProgressionSystem.ShouldSaveLastLevel(sceneName) == false)
            return;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        // sauver le dernier niveau joué 
        LevelProgressionSystem.SetLastLevel(currentIndex);

        // débloquer le niveau suivant
        LevelProgressionSystem.UnlockNextLevel(currentIndex);
    }
}
