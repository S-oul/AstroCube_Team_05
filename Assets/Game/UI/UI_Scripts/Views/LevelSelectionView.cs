using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionView : UIView
{
    [Header("UI")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private LevelItemUI itemPrefab;

    [Header("Levels")]
    [SerializeField] private List<string> levelNames;

    private UIManager _uiManager;
    private List<LevelItemUI> _spawnedItems = new();

    private int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        _uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        GenerateList();
    }

    // -------------------------------
    //      GENERATION DE LA LISTE
    // -------------------------------
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

        // Sélectionne le 1er niveau débloqué
        for (int i = 0; i < _spawnedItems.Count; i++)
        {
            if (_spawnedItems[i].Button.interactable)
            {
                currentIndex = i;
                SelectItem(currentIndex);
                break;
            }
        }
    }

    // -------------------------------
    //      NAVIGATION CLAVIER/MOLETTE
    // -------------------------------
    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        // Navigation flèches
        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveSelection(+1);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveSelection(-1);

        // Molette souris
        if (Input.mouseScrollDelta.y < 0)
            MoveSelection(+1);

        if (Input.mouseScrollDelta.y > 0)
            MoveSelection(-1);
    }

    private void MoveSelection(int direction)
    {
        int newIndex = currentIndex + direction;

        // Clamp = impossible de sortir de la liste
        newIndex = Mathf.Clamp(newIndex, 0, _spawnedItems.Count - 1);

        if (newIndex == currentIndex)
            return;

        // Level lock ? on ignore
        if (!_spawnedItems[newIndex].Button.interactable)
            return;

        currentIndex = newIndex;
        SelectItem(currentIndex);
    }

    private void SelectItem(int index)
    {
        var item = _spawnedItems[index];

        // Focus UI
        item.Button.Select();

        // Auto-scroll
        ScrollTo(index);
    }

    // -------------------------------
    //      AUTO SCROLL SUR SELECTION
    // -------------------------------
    private void ScrollTo(int index)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform item = _spawnedItems[index].GetComponent<RectTransform>();
        RectTransform content = contentRoot.GetComponent<RectTransform>();

        float viewportHeight = scrollRect.viewport.rect.height;
        float contentHeight = content.rect.height;

        float itemPosY = Mathf.Abs(item.anchoredPosition.y);

        float normalizedPos = Mathf.Clamp01(1f - (itemPosY / (contentHeight - viewportHeight)));

        scrollRect.verticalNormalizedPosition = normalizedPos;
    }

    // -------------------------------
    //      ON CLICK LEVEL
    // -------------------------------
    private void OnLevelClicked(int levelIndex)
    {
        Debug.Log("Level selected: " + levelIndex);

        EventManager.TriggerLevelSelected(levelIndex);
    }

    // -------------------------------
    //      OVERRIDE SHOW : refresh
    // -------------------------------
    public override void Show()
    {
        base.Show();

        GenerateList(); // refresh si progression change
    }
}
