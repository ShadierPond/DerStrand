using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractableObject : MonoBehaviour
{
    private enum ObjectType
    {
        None,
        Chest,
    }
    [SerializeField] private GameObject chestInterface;
    [SerializeField] private ObjectType objectType;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isOpen;  
    [SerializeField] private float chestTransitionTime;
    
    public void Interact()
    {
        if (isInteractable)
        {
            switch (objectType)
            {
                case ObjectType.Chest:
                    Debug.Log("Chest opened");
                    isOpen = !isOpen;
                    StartCoroutine(ChestAnimationTransition());
                    break;
            }
        }
    }
    
    private IEnumerator ChestAnimationTransition()
    {
        var chestCanvas = chestInterface.GetComponent<CanvasGroup>();
        if(isOpen)
        {
            LockMouse(false);
            Debug.Log("animating chest open");
            chestCanvas.alpha = 0;
            chestInterface.SetActive(true);
            chestCanvas.DOFade(1, chestTransitionTime);
            yield return new WaitForSeconds(chestTransitionTime);
        }
        else
        {
            LockMouse(true);
            Debug.Log("animating chest close");
            chestCanvas.alpha = 1;
            chestCanvas.DOFade(0, chestTransitionTime);
            yield return new WaitForSeconds(chestTransitionTime);
            chestInterface.SetActive(false);
        }
    }

    private void LockMouse(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }

}
