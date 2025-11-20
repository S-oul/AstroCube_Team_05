using UnityEngine;

public static class LevelProgressionSystem
{
    private const string Key = "LevelUnlocked_";

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


    public static void UnlockAllLevels(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            PlayerPrefs.SetInt(Key + i, 1);
        }

        PlayerPrefs.Save();
    }


    public static void LockAllLevelsExceptFirst(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            if (i == 0)
                PlayerPrefs.SetInt(Key + i, 1);  
            else
                PlayerPrefs.SetInt(Key + i, 0);  
        }

        PlayerPrefs.Save();
    }

    public static void ResetProgression(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            PlayerPrefs.DeleteKey(Key + i);
        }

        PlayerPrefs.Save();
    }
}
