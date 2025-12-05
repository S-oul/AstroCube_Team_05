using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelItemUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private Button button;

    private int levelIndex;
    private System.Action<int> onClicked;

    public void Setup(int index, string levelName, bool unlocked, System.Action<int> callback)
    {
        levelIndex = index;
        onClicked = callback;

        levelNameText.text = SceneUtility.GetScenePathByBuildIndex(levelIndex).Split('/')[^1].Split('.')[0];

        button.interactable = unlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClicked?.Invoke(levelIndex));
    }

    public Button Button => button;
}
