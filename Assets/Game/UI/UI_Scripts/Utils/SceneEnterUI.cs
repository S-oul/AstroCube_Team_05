using UnityEngine;

public class SceneEnterUI : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        uiManager.ShowInGame<PlayingView>();
    }
}
