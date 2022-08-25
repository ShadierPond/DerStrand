using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField] private GameObject[] menus;
    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject currentMenu;
    [SerializeField] private GameObject nextMenu;
    public float transitionTime;
    public float waitTimeBetweenTransitions;
    private CanvasGroup _currentCanvasGroup;
    private CanvasGroup _nextCanvasGroup;
    private  CanvasGroup _inGameCanvasGroup;

    public void Transition(EnumMenuState state)
    {
        foreach (var menu in menus)
        {
            if(menu.name == state.currentMenuState.ToString())
            {
                currentMenu = menu;
                _currentCanvasGroup = currentMenu.GetComponent<CanvasGroup>();
            }
            if(menu.name == state.nextMenuState.ToString())
            {
                nextMenu = menu;
                _nextCanvasGroup = nextMenu.GetComponent<CanvasGroup>();
            }
        }
        StartCoroutine(TransitionWithTime(transitionTime, waitTimeBetweenTransitions));
    }

    private IEnumerator TransitionWithTime(float duration, float wait)
    {
        _nextCanvasGroup.alpha = 0;
        _nextCanvasGroup.interactable = true;
        nextMenu.SetActive(true);
        
        _currentCanvasGroup.DOFade(0, duration / 2);
        yield return new WaitForSeconds(duration / 2);
        currentMenu.SetActive(false);
        _currentCanvasGroup.interactable = false;
        
        yield return new WaitForSeconds(wait);
        
        _nextCanvasGroup.DOFade(1, duration / 2);
        yield return new WaitForSeconds(duration / 2);
    }
    

    public void ExitMenu()
    {
        _inGameCanvasGroup = inGameMenu.GetComponent<CanvasGroup>();
        _inGameCanvasGroup.alpha = 1;

        StartCoroutine(ExitMenuTransition(transitionTime));

    }

    private IEnumerator ExitMenuTransition(float duration)
    {
        _inGameCanvasGroup.DOFade(0, duration);
        yield return new WaitForSeconds(duration);
        inGameMenu.SetActive(false);
        _inGameCanvasGroup.interactable = false;
    }

    public void ReturnToMenu()
    {
        _inGameCanvasGroup = inGameMenu.GetComponent<CanvasGroup>();
        _inGameCanvasGroup.alpha = 0;
        inGameMenu.SetActive(true);
        StartCoroutine(ReturnToMenuTransition(transitionTime));
    }
    
    private IEnumerator ReturnToMenuTransition(float duration)
    {
        _inGameCanvasGroup.DOFade(1, duration);
        yield return new WaitForSeconds(duration);
        _inGameCanvasGroup.interactable = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void ReturnToMainMenu()
    {
        // TODO: Add a confirmation dialog and a fade out animation for the current scene before loading the main menu scene
    }
}