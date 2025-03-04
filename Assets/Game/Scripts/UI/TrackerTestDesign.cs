using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrackerTestDesign : MonoBehaviour
{
    private int rotationTracker = 0;
    private float timeTracker = 0f;

    [SerializeField] private TextMeshProUGUI rotationTrackerText;
    [SerializeField] private TextMeshProUGUI timeTrackerText;

    private bool isTimerRunning = false;



    private void OnEnable()
    {
        EventManager.OnSceneStart += StartTimer;
        EventManager.OnSceneEnd += StopTimer;
        EventManager.OnCubeRotated += IncrementRotation;
    }

    private void OnDisable()
    {
        EventManager.OnSceneStart -= StartTimer;
        EventManager.OnSceneEnd -= StopTimer;
        EventManager.OnCubeRotated -= IncrementRotation;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timeTracker += Time.deltaTime;
            UpdateUI();
        }
    }

    private void StartTimer()
    {
        isTimerRunning = true;
    }

    private void StopTimer()
    {
        isTimerRunning = false;
    }

    private void ResetTracker()
    {
        isTimerRunning = false;
        timeTracker = 0f;
        rotationTracker = 0;
        UpdateUI();
    }

    private void IncrementRotation()
    {
        rotationTracker++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timeTrackerText != null)
            timeTrackerText.text = "Time : " + timeTracker.ToString("F2") + "s";

        if (rotationTrackerText != null)
            rotationTrackerText.text = "Rotations : " + rotationTracker;
    }
}
