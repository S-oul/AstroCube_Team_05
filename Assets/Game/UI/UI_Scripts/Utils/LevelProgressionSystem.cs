using UnityEngine;

public static class LevelProgressionSystem
{
    private const string Key = "LevelUnlocked_";
    private const string LastLevelKey = "LastLevelPlayed";

    public static bool IsUnlocked(int levelIndex)
    {
        if (levelIndex == 0)
            return true;

        return PlayerPrefs.GetInt(Key + levelIndex, 0) == 1;
    }

    public static void Unlock(int levelIndex)
    {
        PlayerPrefs.SetInt(Key + levelIndex, 1);
        PlayerPrefs.Save();
    }

    public static void UnlockNextLevel(int currentLogicalLevel)
    {
        int next = currentLogicalLevel + 1;
        PlayerPrefs.SetInt(Key + next, 1);
        PlayerPrefs.Save();
    }

    public static void LockAllLevelsExceptFirst(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
            PlayerPrefs.SetInt(Key + i, (i == 0) ? 1 : 0);

        PlayerPrefs.Save();
    }

    public static void ResetProgression(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
            PlayerPrefs.DeleteKey(Key + i);

        PlayerPrefs.Save();
    }


    public static void SetLastLevel(int logicalLevel)
    {
        PlayerPrefs.SetInt(LastLevelKey, logicalLevel);
        PlayerPrefs.Save();
    }

    public static void ResetLastLevel()
    {
        PlayerPrefs.DeleteKey(LastLevelKey);
        PlayerPrefs.Save();
    }

    public static int LogicalToSceneIndex(int logicalLevel)
    {
        return logicalLevel + 1;
    }


    public static int GetLastLevel()
    {
        return PlayerPrefs.GetInt(LastLevelKey, -1);
    }

    public static bool HasProgression()
    {
        return PlayerPrefs.HasKey(LastLevelKey);
    }

    public static bool ShouldSaveLastLevel(string sceneName)
    {
        return sceneName != "GameEntry";
    }
}
