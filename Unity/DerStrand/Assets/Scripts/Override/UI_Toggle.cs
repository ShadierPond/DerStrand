using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class UI_Toggle : MonoBehaviour
{
    [SerializeField] private bool state;
    [SerializeField] private GameObject handle;
    
    public void SwitchState()
    {
        state = !state;
        handle.transform.DOLocalMoveX(-handle.transform.localPosition.x, 0.2f);
        GetComponent<UnityEngine.UI.Toggle>().isOn = state;
    }
}
