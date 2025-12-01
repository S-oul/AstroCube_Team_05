using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressionListener : MonoBehaviour
{
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

        if (!LevelProgressionSystem.ShouldSaveLastLevel(sceneName))
            return;

        int logicalIndex = SceneManager.GetActiveScene().buildIndex;
        logicalIndex -= 1; 

        LevelProgressionSystem.SetLastLevel(logicalIndex);
        LevelProgressionSystem.UnlockNextLevel(logicalIndex);
    }
}
