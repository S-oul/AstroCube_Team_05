using System;
using System.Collections;
using UnityEngine;
using Steamworks;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }
    
    private float _currentTime;
    private int _currentAmountOfRotations;
    
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
    
    private void Start()
    {
        _currentTime = PlayerPrefs.HasKey("CURRENT_TIME") ? PlayerPrefs.GetFloat("CURRENT_TIME") : 0f;
        _currentAmountOfRotations = PlayerPrefs.HasKey("ROTATION_AMOUNT") ? PlayerPrefs.GetInt("ROTATION_AMOUNT") : 0;

        EventManager.OnLevelFinished += EndGame;
        
        StartCoroutine(SaveCoroutine());
    }

    private void OnDestroy()
    {
        EventManager.OnLevelFinished -= EndGame;
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
    
    private void Update()
    {
        _currentTime += Time.deltaTime;
    }
    
    private IEnumerator SaveCoroutine()
    {
        while (gameObject.activeSelf)
        {
            PlayerPrefs.SetFloat("CURRENT_TIME", _currentTime);
            PlayerPrefs.SetInt("ROTATION_AMOUNT", _currentAmountOfRotations);
            
            Debug.LogWarning($"Saved : Time = {PlayerPrefs.GetFloat("CURRENT_TIME")}, Rotations = {PlayerPrefs.GetInt("ROTATION_AMOUNT")}");
            
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public void EndGame()
    {
        if(_currentTime <= GameManager.Instance.Settings.MaxTimeForAchievement)
            UnlockAchievement("SPEEDRUNNER");
        
        if(_currentAmountOfRotations <= GameManager.Instance.Settings.MaxRotationsesForAchievement)
            UnlockAchievement("EFFICIENCY");
    }

    public void AddRotation()
    {
        _currentAmountOfRotations++;
    }
}
