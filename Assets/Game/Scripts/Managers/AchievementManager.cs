using System;
using System.Collections;
using UnityEngine;
using Steamworks;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }
    
    private float _currentTime;
    private int _currentAmountOfRotations;
    private int _lockedRotations;
    
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
            PlayerPrefs.SetInt("LOCKED_ROTATIONS", _lockedRotations);
            
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public void EndGame()
    {
        if(_currentTime <= GameManager.Instance.Settings.MaxTimeForAchievement)
            UnlockAchievement("SPEEDRUNNER");
        
        if(_currentAmountOfRotations <= GameManager.Instance.Settings.MaxRotationsForAchievement)
            UnlockAchievement("EFFICIENCY");
    }

    public void AddRotation(bool locked = false)
    {
        if (locked)
        {
            _lockedRotations++;
            if (_lockedRotations > GameManager.Instance.Settings.MaxLockedRotationsForAchievement)
            {
                UnlockAchievement("DOOR_STUCK");
            }
        }
        else
        {
            _currentAmountOfRotations++;
        }
    }
}
