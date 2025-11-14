using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelItemUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockIcon;

    private int levelIndex;
    private System.Action<int> onClicked;

    /// <summary>
    /// Initialise l’item avec son index, son nom et son état (lock/unlock)
    /// </summary>
    public void Setup(int index, string levelName, bool unlocked, System.Action<int> callback)
    {
        levelIndex = index;
        onClicked = callback;

        levelNameText.text = levelName;

        lockIcon.SetActive(!unlocked);
        button.interactable = unlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClicked?.Invoke(levelIndex));
    }

    public Button Button => button;
}
