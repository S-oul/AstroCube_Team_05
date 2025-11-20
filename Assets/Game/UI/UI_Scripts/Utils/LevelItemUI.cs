using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        levelNameText.text = levelName;

        button.interactable = unlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClicked?.Invoke(levelIndex));
    }

    public Button Button => button;
}
