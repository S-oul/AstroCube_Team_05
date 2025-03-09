using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;
    [SerializeField] bool PToReturnToMenu = true;

    private void Awake()
    {
        EventManager.OnPlayerWin += ChangeScener;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerWin -= ChangeScener;
    }
    private void OnDestroy()
    {
        EventManager.OnPlayerWin -= ChangeScener;
    }
    public void ChangeScener()
    {
        Cursor.lockState = CursorLockMode.Confined;
        EventManager.OnPlayerWin -= ChangeScener;
        SceneManager.LoadScene(scene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && PToReturnToMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            EventManager.TriggerSceneEnd();
            SceneManager.LoadScene("StartMenu");
        }
    }
}
