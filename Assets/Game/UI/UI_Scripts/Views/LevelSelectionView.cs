using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionView : UIView
{
    [Header("UI")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private LevelItemUI itemPrefab;

    [Header("Levels")]
    [SerializeField] private List<string> levelNames;

    [Header("Scrolling")]
    [SerializeField] private float scrollSpeed = 10f;

    private List<LevelItemUI> _spawnedItems = new();
    private int currentIndex = 0;
    private float targetScrollPos = 1f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GenerateList();
    }

    private void GenerateList()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        _spawnedItems.Clear();

        for (int i = 0; i < levelNames.Count; i++)
        {
            var item = Instantiate(itemPrefab, contentRoot);
            bool unlocked = LevelProgressionSystem.IsUnlocked(i);

            item.Setup(i, levelNames[i], unlocked, OnLevelClicked);

            _spawnedItems.Add(item);
        }
    }

    public override void Show()
    {
        base.Show();
        GenerateList();
        StartCoroutine(SelectFirstNextFrame());
    }

    private System.Collections.IEnumerator SelectFirstNextFrame()
    {
        yield return null;

        if (_spawnedItems.Count > 0)
        {
            currentIndex = 0;

            _spawnedItems[0].Button.Select();
            _spawnedItems[0].Button.OnSelect(null);

            ScrollTo(0);
        }
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveSelection(+1);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveSelection(-1);

        if (Input.mouseScrollDelta.y < 0)
            MoveSelection(+1);

        if (Input.mouseScrollDelta.y > 0)
            MoveSelection(-1);

        scrollRect.verticalNormalizedPosition =
            Mathf.Lerp(scrollRect.verticalNormalizedPosition, targetScrollPos, Time.deltaTime * scrollSpeed);
    }

    private void MoveSelection(int direction)
    {
        if (_spawnedItems.Count == 0) return;

        int newIndex = Mathf.Clamp(currentIndex + direction, 0, _spawnedItems.Count - 1);

        if (newIndex == currentIndex)
            return;

        if (!_spawnedItems[newIndex].Button.interactable)
            return;

        currentIndex = newIndex;
        SelectItem(currentIndex);
    }

    private void SelectItem(int index)
    {
        var item = _spawnedItems[index];

        item.Button.Select();
        ScrollTo(index);
    }

    private void ScrollTo(int index)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform item = _spawnedItems[index].GetComponent<RectTransform>();
        RectTransform content = contentRoot.GetComponent<RectTransform>();

        float viewportHeight = scrollRect.viewport.rect.height;
        float contentHeight = content.rect.height;

        float itemY = Mathf.Abs(item.anchoredPosition.y);

        float target =
            1f - ((itemY - (viewportHeight * 0.5f) + (item.rect.height * 0.5f))
            / (contentHeight - viewportHeight));

        targetScrollPos = Mathf.Clamp01(target);
    }

    private void OnLevelClicked(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelNames.Count)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }

        string sceneName = levelNames[levelIndex];

        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Scene name is null or empty for index " + levelIndex);
            return;
        }

        Debug.Log("Loading level: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

}
