using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInterface : MonoBehaviour
{
    [SerializeField] private GameObject[] interfaceObjects;
    [SerializeField] private GameObject currentInterface;
    [SerializeField] private CanvasGroup currentInterfaceCanvasGroup;
    [SerializeField] private GameObject nextInterface;
    [SerializeField] private CanvasGroup nextInterfaceCanvasGroup;
    public float transitionTime;
    public float waitTimeBetweenTransitions;
}
