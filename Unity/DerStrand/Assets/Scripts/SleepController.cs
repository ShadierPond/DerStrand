using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SleepController : MonoBehaviour
{
    [SerializeField] private GameObject sleepPanel;
    [SerializeField] private Button sleepButton;
    [SerializeField] private TMP_Text hoursText;
    [SerializeField] private int hours;
    [SerializeField] private Button addHourButton;
    [SerializeField] private Button subtractHourButton;
    [SerializeField] private float sleepTime;
    [SerializeField] private float sleepTransitionTime;

    private void Start()
    {
        addHourButton.onClick.AddListener(() =>
        {
            hours++;
            UpdateUI();
        });
        subtractHourButton.onClick.AddListener(() =>
        {
            hours--;
            UpdateUI();
        });
        sleepButton.onClick.AddListener(() => StartCoroutine(Sleep()));
    }
    
    private void UpdateUI()
    {
        hoursText.text = math.clamp(hours, 1, 24).ToString();
    }
    
    
    private IEnumerator Sleep()
    {
        var canvasGroup = GameObject.Find("CrossFade").GetComponent<CanvasGroup>();
        if(!canvasGroup)
            yield break;
        
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, sleepTransitionTime);
        yield return new WaitForSeconds(sleepTransitionTime);
        SetStats();
        yield return new WaitForSeconds(sleepTime);
        canvasGroup.DOFade(0, sleepTransitionTime);
        yield return new WaitForSeconds(sleepTransitionTime);
    }

    private void SetStats()
    {
        
    }
}

