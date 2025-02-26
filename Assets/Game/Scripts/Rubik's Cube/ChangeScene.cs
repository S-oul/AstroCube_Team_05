using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;
    [SerializeField] bool PToReturnToMenu = true;

    public void ChangeScener()
    {
        Debug.Log("Changing Scene to " + scene);
        SceneManager.LoadScene(scene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("StartMenu");
        }
    }
}
