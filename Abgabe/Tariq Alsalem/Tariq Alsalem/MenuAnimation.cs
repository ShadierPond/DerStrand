using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class MenuAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float transitionTime;
    public float waitTimeBetweenTransitions;
    
    [Header("Controllable Objects")]
    [SerializeField] private GameObject[] menus;
    
    [Header("Menu Parents")]
    [SerializeField] private GameObject inGameMenu;
    private  CanvasGroup _inGameCanvasGroup;
    [SerializeField] private GameObject interfaceMenu;
    private  CanvasGroup _interfaceCanvasGroup;
    
    [Header("Debug")]
    [SerializeField] private GameObject currentMenu;
    private CanvasGroup _currentCanvasGroup;
    [SerializeField] private GameObject nextMenu;
    private CanvasGroup _nextCanvasGroup;
    [SerializeField] private bool isMenuOpen;
    [SerializeField] private bool isMenuLocked;
    [SerializeField] private bool isInterfaceMenuOpen;
    [SerializeField] private GameObject lookAtInterface;

    // Transition from one menu to another
    public void Transition(EnumMenuState state)
    {
        if(state.isInterface)
            foreach (Transform child in interfaceMenu.transform)
                if (child.gameObject.activeSelf)
                {
                    currentMenu = child.gameObject;
                    _currentCanvasGroup = currentMenu.GetComponent<CanvasGroup>();
                }
        
        foreach (var menu in menus)
        {
            if(menu.name == state.currentMenuState.ToString() && !state.isInterface)
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
    // Transition from one menu to another with time
    private IEnumerator TransitionWithTime(float duration, float wait)
    {
        if (currentMenu == nextMenu)
        {
            yield break;
        }
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
    // Open and close the in game menu
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
    // Open and close the interface menu
    public void InInterfaceMenu(bool state)
    {
        isInterfaceMenuOpen = state;
        _interfaceCanvasGroup = interfaceMenu.GetComponent<CanvasGroup>();
        if(state)
        {
            _interfaceCanvasGroup.alpha = 0;
            interfaceMenu.SetActive(true);
            StartCoroutine(InterfaceTransition(true, transitionTime));
        }
        else
        {
            _interfaceCanvasGroup.alpha = 1;
            StartCoroutine(InterfaceTransition(false, transitionTime));
        }
    }
    // Transition the in game menu menus
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
    // Transition the interface menu menus
    private IEnumerator InterfaceTransition(bool state, float duration)
    {
        if (!state)
        {
            _interfaceCanvasGroup.DOFade(0, duration);
            yield return new WaitForSeconds(duration);
        }
        _interfaceCanvasGroup.interactable = state;
        interfaceMenu.SetActive(state);
        if (state)
        {
            _interfaceCanvasGroup.DOFade(1, duration);
            yield return new WaitForSeconds(duration);
        }
    }
    // Open and close the ingame menu (input)
    public void PlayerMenu(InputAction.CallbackContext context)
    {
        if(!context.performed || isInterfaceMenuOpen)
            return;
        
        if (isMenuOpen && !isMenuLocked)
        {
            PauseGame(false);
            InGameMenu(false);
            LockMouse(true);
        }
        else if(!isMenuOpen && !isMenuLocked)
        {
            PauseGame(true);
            InGameMenu(true);
            LockMouse(false);
        }
    }
    // Open and close the interface menu (input)
    public void PlayerInterface(InputAction.CallbackContext context)
    {
        if(!context.performed || isMenuOpen)
            return;
        
        if (isInterfaceMenuOpen)
        {
            PauseGame(false);
            InInterfaceMenu(false);
            LockMouse(true);
        }
        else
        {
            PauseGame(true);
            InInterfaceMenu(true);
            LockMouse(false);
        }
    }
    // Lock and unlock the mouse
    public void LockMouse(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }
    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
    // Return to the main menu
    public void ReturnToMainMenu()
    {
        GameManager.Instance.LoadScene("Main Menu");
    }
    
    public void LookAtInteractable()
    {
        lookAtInterface.SetActive(true);
    }
    public void NotLookAtInteractable()
    {
        lookAtInterface.SetActive(false);
    }
    
    public void PauseGame(bool state)
    {
        GameManager.Instance.PauseGame(state);
    }
}