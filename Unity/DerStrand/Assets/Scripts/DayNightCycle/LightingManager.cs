using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("Lighting Settings")]
    // Sun Light Object in Game Scene
    [SerializeField] private Light directionalLight;
    // Lighting Preset
    [SerializeField] private LightingPreset preset;
    // Time in 24 Hour Format (minutes in Commas)
    [SerializeField, Range(0, 24)] private float timeOfDay;
    // Time in String Format
    [SerializeField] private string timeOfDayString;
    // In-Game Days
    [SerializeField, Rename("In-Game Days")] private int day;
    // Days length in Minutes
    [SerializeField, Rename("Day Length in Minutes")] private float dayLength;
    
    [Header("UI Settings")]
    // UI Text Object for Time
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    // UI Text Object for Day
    [SerializeField] private TMPro.TextMeshProUGUI dayText;
    
    [Header("Save Settings")]
    // Save Data
    [SerializeField] private SaveData saveData;
    // Public access to Class
    public static LightingManager Instance { get; private set; }
    
    // Set the Instance
    private void Awake()
    {
        Instance = this;
    }
    
    // Save Data
    public void Save()
    {
        // Save Time
        saveData.startTime = timeOfDay;
        // Save Day
        saveData.daysSurvived = day;
    }
    
    // Load Data
    private void Load()
    {
        // Load Time
        timeOfDay = saveData.startTime;
        // Load Day
        day = saveData.daysSurvived;
    }

    private void Start()
    {
        // If the Application is not running, 
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
