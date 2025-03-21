using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GoToNextScene : MonoBehaviour
{
    VideoPlayer _videoPlayer;

    private void OnEnable()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += ChangeScene;
    }

    void ChangeScene(VideoPlayer source)
    {
        SceneManager.LoadScene(1);
    }
}
