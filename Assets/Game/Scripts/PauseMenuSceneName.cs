using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuSceneName : MonoBehaviour
{
    void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name;
    }
}
