using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelectionView : UIView
{
    [Header("UI")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private LevelItemUI itemPrefab;
    [SerializeField] private Button backButton;
    [SerializeField] private Image previewImage;
    [SerializeField] private List<Sprite> previewSprites;

    [Header("Levels")]
    [SerializeField] private List<string> levelNames;

    [Header("Scrolling")]
    [SerializeField] private float scrollSpeed = 10f;

    private List<LevelItemUI> items = new();
    private int currentIndex = 0;
    private float targetScrollPos = 1f;

    private UIManager uiManager;

    protected override void Awake()
    {
        base.Awake();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        GenerateList();
        backButton.onClick.AddListener(OnBackClicked);
    }

public void unlockAllLevels(){
LevelProgressionSystem.UnlockAllLevels(12);
}

    private void GenerateList()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        items.Clear();

        for (int i = 0; i < levelNames.Count; i++)
        {
            var item = Instantiate(itemPrefab, contentRoot);
            bool unlocked = LevelProgressionSystem.IsUnlocked(i);

            item.Setup(i, levelNames[i], unlocked, OnLevelClicked);
            items.Add(item);
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

        currentIndex = 0;

        items[0].Button.Select();
        ScrollTo(0);
        UpdatePreview(0);
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        UpdateIndexFromUnityNavigation();
        SmoothScroll();
        AutoFocusIfLost();
    }

    private void UpdateIndexFromUnityNavigation()
    {
        var sel = EventSystem.current.currentSelectedGameObject;
        if (sel == null)
            return;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Button.gameObject == sel)
            {
                if (currentIndex != i)
                {
                    currentIndex = i;
                    ScrollTo(i);
                    UpdatePreview(i);
                }
                return;
            }
        }
    }

    private void AutoFocusIfLost()
    {
        var sel = EventSystem.current.currentSelectedGameObject;

        if (sel != null)
            return;

        items[currentIndex].Button.Select();
    }

    private void SmoothScroll()
    {
        scrollRect.verticalNormalizedPosition =
            Mathf.Lerp(scrollRect.verticalNormalizedPosition, targetScrollPos, Time.deltaTime * scrollSpeed);
    }

    private void ScrollTo(int index)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform item = items[index].GetComponent<RectTransform>();
        RectTransform content = contentRoot.GetComponent<RectTransform>();

        float viewport = scrollRect.viewport.rect.height;
        float contentHeight = content.rect.height;
        float itemY = Mathf.Abs(item.anchoredPosition.y);

        float target = 1f - ((itemY - (viewport * 0.5f) + (item.rect.height * 0.5f))
                             / (contentHeight - viewport));

        targetScrollPos = Mathf.Clamp01(target);
    }

    private void UpdatePreview(int index)
    {
        if (index < 0 || index >= previewSprites.Count)
            return;

        previewImage.sprite = previewSprites[index];
    }

    private void OnLevelClicked(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index);
    }

    private void OnBackClicked()
    {
        Hide();
        uiManager.Show<MainMenuView>();
    }
}
