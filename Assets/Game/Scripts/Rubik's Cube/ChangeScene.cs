using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public ChangeScene Instance;

    public string scene;
    [SerializeField] bool PToReturnToMenu = true;

    private void Awake()
    {
        EventManager.OnPlayerWin += ChangeScener;
    }
    public void ChangeScener()
    {
        Debug.Log("Changing Scene to " + scene);
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
