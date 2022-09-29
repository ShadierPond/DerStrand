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
    [SerializeField, Range(0, 24)] public float timeOfDay;
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
        // If the Application is not running, return
        if (!Application.isPlaying) 
            return;
        // Get the Save Data
        saveData = SaveSystem.Instance.saveData;
        // if the Game is not New, Load the Data
        if (!SaveSystem.Instance.newGame)
            Load();
        else
        {
            // Standard Values for new Game
            day = 1;
            timeOfDay = 12;
        }
    }

    private void Update()
    {
        // if there is no Lighting Preset, return
        if (!preset)
            return;
        // if the Application is running
        if (Application.isPlaying)
        {
            // Add Time to the Time of Day
            timeOfDay += Time.deltaTime / dayLength;
            // if the Time of Day is greater than 24
            if (timeOfDay >= 24)
            {
                // Add a Day
                day++;
                // Reset the Time of Day to 0
                timeOfDay %= 24;
            }
        }
        UpdateLighting(timeOfDay / 24);
        UpdateUI();
    }
    
    // Update the Lighting
    private void UpdateLighting(float timePercent)
    {
        // Set the ambient color
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        // Set the fog Color
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);
        // Set the Directional Light Color
        directionalLight.color = preset.directionalColor.Evaluate(timePercent);
        // Rotate the Directional Light
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
    }

    private void UpdateUI()
    {
        // Set the Text for the Time
        timeOfDayString = $"{(int)timeOfDay:00}:{(int)(timeOfDay * 60) % 60:00}";
        // if the Text Object is null, return
        if (!dayText || !timeText) 
            return;
        // Set the Day text
        dayText.text = "Day: " + day;
        // Set the Time text
        timeText.text = "Time: " + timeOfDayString;
    }
}
