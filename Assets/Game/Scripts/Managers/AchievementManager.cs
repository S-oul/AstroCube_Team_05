using System;
using System.Collections;
using UnityEngine;
using Steamworks;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        
        #if UNITY_EDITOR
        if (SteamManager.Initialized)
        {
            SteamUserStats.ResetAllStats(true);
        }
        #endif
    }

    public void UnlockAchievement(string key)
    {
        if (!SteamManager.Initialized)
            return;

        SteamUserStats.GetAchievement(key, out bool achieved);
        if (!achieved)
        {
            SteamUserStats.SetAchievement(key);
        }
    }
}
