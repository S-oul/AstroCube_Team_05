using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField][Scene] int NextSceneToLoad;


    [Button]
    public void LoadScene()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    IEnumerator LoadSceneCoroutine()
    {
        LoadSceneParameters param = new()
        {
            loadSceneMode = LoadSceneMode.Additive,
            localPhysicsMode = LocalPhysicsMode.Physics3D
        };


        var load = SceneManager.LoadSceneAsync(NextSceneToLoad, param);
        while (!load.isDone)
        {
            print("Percent : " + load.progress *100+ "%");
            yield return null;
        }

        print("Finish loaded scene");


    }

}
