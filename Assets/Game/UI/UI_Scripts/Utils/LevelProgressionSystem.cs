using UnityEngine;

public static class LevelProgressionSystem
{
    // Clé PlayerPrefs
    private const string Key = "LevelUnlocked_";

    /// <summary>
    /// Vérifie si un niveau est débloqué.
    /// Le niveau 0 (premier) est toujours débloqué.
    /// </summary>
    public static bool IsUnlocked(int levelIndex)
    {
        if (levelIndex == 0)
            return true;

        return PlayerPrefs.GetInt(Key + levelIndex, 0) == 1;
    }

    /// <summary>
    /// Débloque un niveau dans les PlayerPrefs.
    /// </summary>
    public static void Unlock(int levelIndex)
    {
        PlayerPrefs.SetInt(Key + levelIndex, 1);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Réinitialise toute la progression.
    /// </summary>
    public static void ResetProgression()
    {
        for (int i = 0; i < 200; i++)
            PlayerPrefs.DeleteKey(Key + i);

        PlayerPrefs.Save();
    }
}
