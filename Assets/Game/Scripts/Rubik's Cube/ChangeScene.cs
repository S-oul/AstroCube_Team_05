using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;
    public void ChangeScener()
    {
        SceneManager.LoadScene(scene);
    }
}
