using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("Lighting Settings")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField] private string timeOfDayString;
    [SerializeField, Rename("In-Game Days")] private int day;
    [SerializeField, Rename("Day Length in Minutes")] private float dayLength;
    
    [Header("UI Settings")]
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI dayText;
    
    [Header("Save Settings")]
    [SerializeField] private SaveData saveData;
    public static LightingManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Save()
    {
        saveData.startTime = timeOfDay;
        saveData.daysSurvived = day;
    }
    
    private void Load()
    {
        timeOfDay = saveData.startTime;
        day = saveData.daysSurvived;
    }

    private void Start()
    {
        if (!Application.isPlaying) 
            return;
        saveData = SaveSystem.Instance.saveData;
        if (!SaveSystem.Instance.newGame)
            Load();
        else
        {
            day = 1;
            timeOfDay = 12;
        }
    }

    private void Update()
    {
        if (!preset)
            return;
        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime / dayLength;
            if (timeOfDay >= 24)
            {
                day++;
                timeOfDay %= 24;
            }
        }
        UpdateLighting(timeOfDay / 24);
        UpdateUI();
    }
    
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);
        directionalLight.color = preset.directionalColor.Evaluate(timePercent);
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
    }

    private void UpdateUI()
    {
        timeOfDayString = $"{(int)timeOfDay:00}:{(int)(timeOfDay * 60) % 60:00}";
        if (!dayText || !timeText) 
            return;
        dayText.text = "Day: " + day;
        timeText.text = "Time: " + timeOfDayString;
    }
}
