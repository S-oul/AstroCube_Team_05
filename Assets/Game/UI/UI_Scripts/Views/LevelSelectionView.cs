using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using System.Collections;

public class LevelSelectionView : UIView
{
    [Header("UI")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private LevelItemUI itemPrefab;
    [SerializeField] private Button backButton;
    [SerializeField] private RawImage previewImage;
    [SerializeField] private List<Material> previewMaterials;
    [SerializeField] private Material lockedPreviewMaterial;
    [SerializeField] private CanvasGroup levelListCanvasGroup;


    [Header("Levels")]
    [SerializeField] private List<string> levelNames;
    [SerializeField] private int levelIndexOffset = 1;

    [Header("Scrolling")]
    [SerializeField] private float scrollSpeed = 10f;

    private List<LevelItemUI> items = new();
    private int currentIndex = 0;
    private float targetScrollPos = 1f;

    private UIManager uiManager;

    private InputAction _cancelAction;


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

    private void OnEnable()
    {
        var uiModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        _cancelAction = uiModule.cancel;

        _cancelAction.performed += OnCancelPerformed;

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

            TMPro.TMP_Text label = item.GetComponentInChildren<TMPro.TMP_Text>();
            if (label != null)
                label.text = levelNames[i];

            item.Button.interactable = unlocked;

            items.Add(item);
        }

        Canvas.ForceUpdateCanvases();
    }



    public override void Show()
    {
        levelListCanvasGroup.alpha = 0f;
        levelListCanvasGroup.interactable = false;
        levelListCanvasGroup.blocksRaycasts = false;
        base.Show();
        GenerateList();
        StartCoroutine(SelectFirstNextFrame());
        StartCoroutine(ShowListAfterFade());

    }

    private IEnumerator ShowListAfterFade()
    {
        yield return new WaitForSeconds(0.5f);

        levelListCanvasGroup.alpha = 1f;
        levelListCanvasGroup.interactable = true;
        levelListCanvasGroup.blocksRaycasts = true;
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
        HandleMouseWheelScroll();
        SmoothScroll();
        AutoFocusIfLost();
    }

    private void UpdateIndexFromUnityNavigation()
    {
        var sel = EventSystem.current.currentSelectedGameObject;
        if (sel == null)
            return;

        if (sel == backButton.gameObject)
        {
            currentIndex = -1;
            return;
        }

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

    private int GetNextUnlocked(int start, int direction)
    {
        int i = start;

        while (true)
        {
            i += direction;

            if (i < 0 || i >= items.Count)
                return start;

            if (LevelProgressionSystem.IsUnlocked(i))
                return i;
        }
    }

    private void HandleMouseWheelScroll()
    {
        if (Input.mouseScrollDelta.y < 0f)
        {
            int newIndex = GetNextUnlocked(currentIndex, +1);
            if (newIndex != currentIndex)
            {
                currentIndex = newIndex;
                items[currentIndex].Button.Select();
                ScrollTo(currentIndex);
                UpdatePreview(currentIndex);
            }
        }

        if (Input.mouseScrollDelta.y > 0f)
        {
            int newIndex = GetNextUnlocked(currentIndex, -1);
            if (newIndex != currentIndex)
            {
                currentIndex = newIndex;
                items[currentIndex].Button.Select();
                ScrollTo(currentIndex);
                UpdatePreview(currentIndex);
            }
        }
    }

    private void AutoFocusIfLost()
    {
        var sel = EventSystem.current.currentSelectedGameObject;

        if (sel != null)
            return;

        if (currentIndex == -1)
            backButton.Select();
        else
            items[currentIndex].Button.Select();
    }


    private void OnCancelPerformed(InputAction.CallbackContext ctx)
    {
        Hide();
        uiManager.Show<MainMenuView>();
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
        if (index < 0 || index >= previewMaterials.Count)
            return;

        bool unlocked = LevelProgressionSystem.IsUnlocked(index);

        if (!unlocked)
        {
            previewImage.material = lockedPreviewMaterial;
            return;
        }

        previewImage.material = previewMaterials[index];
    }

    private void OnLevelClicked(int index)
    {
        LevelProgressionSystem.Unlock(index);
        LevelProgressionSystem.SetLastLevel(index);
        SceneManager.LoadScene(index+levelIndexOffset);
    }

    private void OnBackClicked()
    {
        HideImmediate();
        uiManager.Show<MainMenuView>();
    }
}
