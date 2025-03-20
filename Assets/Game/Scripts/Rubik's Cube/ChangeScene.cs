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
        EventManager.OnSceneChange += ChangeScener;
    }
    private void OnDisable()
    {
        EventManager.OnSceneChange -= ChangeScener;
    }
    private void OnDestroy()
    {
        EventManager.OnSceneChange -= ChangeScener;
    }
    public void ChangeScener()
    {
        Cursor.lockState = CursorLockMode.Confined;
        EventManager.OnSceneChange -= ChangeScener;
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
