using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField] private GameObject[] menus;
    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject interfaceMenu;
    [SerializeField] private GameObject currentMenu;
    [SerializeField] private GameObject nextMenu;
    [SerializeField] private GameObject lookAtInterface;
    [SerializeField] private bool isMenuOpen;
    [SerializeField] private bool isMenuLocked;
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
                if(menu.name == "Main")
                    isMenuLocked = true;
                _currentCanvasGroup = currentMenu.GetComponent<CanvasGroup>();
            }
            if(menu.name == state.nextMenuState.ToString())
            {
                if(menu.name == "Main")
                    isMenuLocked = false;
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

    public void InGameMenu(bool state)
    {
        isMenuOpen = state;
        _inGameCanvasGroup = inGameMenu.GetComponent<CanvasGroup>();
        if(state)
        {
            _inGameCanvasGroup.alpha = 0;
            inGameMenu.SetActive(true);
            StartCoroutine(InGameMenuTransition(true, transitionTime));
        }
        else
        {
            _inGameCanvasGroup.alpha = 1;
            StartCoroutine(InGameMenuTransition(false, transitionTime));
        }
    }

    private IEnumerator InGameMenuTransition(bool state, float duration)
    {
        if (!state)
        {
            _inGameCanvasGroup.DOFade(0, duration);
            yield return new WaitForSeconds(duration);
        }
        _inGameCanvasGroup.interactable = state;
        inGameMenu.SetActive(state);
        if (state)
        {
            _inGameCanvasGroup.DOFade(1, duration);
            yield return new WaitForSeconds(duration);
        }
    }
    
    public void PlayerMenu(InputAction.CallbackContext context)
    {
        if(!context.performed)
            return;
        
        if (isMenuOpen && !isMenuLocked)
        {
            Debug.Log("Closing");
            InGameMenu(false);
            LockMouse(true);
            Debug.Log("Closed");
        }
        else if(!isMenuOpen && !isMenuLocked)
        {
            Debug.Log("Opening");
            InGameMenu(true);
            LockMouse(false);
            Debug.Log("Opened");
        }
    }

    private void LockMouse(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void ReturnToMainMenu()
    {
        // TODO: Add a confirmation dialog and a fade out animation for the current scene before loading the main menu scene
    }

    //TODO New method for showing the E key if you look at things
    public void LookAtInteractable()
    {
        lookAtInterface.SetActive(true);
    }
    public void NotLookAtInteractable()
    {
        lookAtInterface.SetActive(false);
    }
}