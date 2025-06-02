//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenuElements : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _titleScreen;

    private void Start()
    {
        _mainMenu.SetActive(false);
        _titleScreen.SetActive(true);
    }

    public async void DeactivateTitleScreen()
    {
        await fadeAlpha(_titleScreen.GetComponent<CanvasGroup>(), 0, 1);
        _titleScreen.SetActive(false); 
    }    
    public void ActivateMainMenuScreen()
    {
        _mainMenu.SetActive(true);
        _mainMenu.GetComponent<CanvasGroup>().alpha = 0;
        fadeAlpha(_mainMenu.GetComponent<CanvasGroup>(), 1, 1); 
    }

    async Task fadeAlpha(CanvasGroup uiGroup, float targetAlpha, float duration)
    {
        float elapsedTime = 0;
        float startAlpha = uiGroup.alpha;

        while (elapsedTime < duration)
        {
            uiGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }
    }
}
