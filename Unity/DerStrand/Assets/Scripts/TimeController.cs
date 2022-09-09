using System;
using System.Globalization;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    [Header("Global Settings")]
    [SerializeField] private float timeMultiplier;
    [SerializeField] private float startHour;
    private DateTime currentTime;
    [SerializeField] private int startDay;
    [SerializeField] private float sunriseHour;
    private TimeSpan sunriseTime;
    [SerializeField] private float sunsetHour;
    private TimeSpan sunsetTime;
    [SerializeField] private AnimationCurve lightChangeCurve;
    
    [Header("UI")]
    [SerializeField] private bool showTime;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dayText;
    
    [Header("Sun Settings")]
    [SerializeField] private Light sunLight;
    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private float maxSunLightIntensity;
    
    [Header("Moon Settings")]
    [SerializeField] private Light moonLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private float maxMoonLightIntensity;
    
    [Header("Debug")]
    [SerializeField] private float daysSurvived;
    [SerializeField] private SaveData saveData;
    
    public static TimeController Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public void Save()
    {
        saveData.startTimeString = currentTime.Hour + ":" + currentTime.Minute;
        saveData.daysSurvived = currentTime.Day;
    }

    private void Load()
    {
        startHour = float.Parse(saveData.startTimeString.Split(':')[0]) + (float.Parse(saveData.startTimeString.Split(':')[1]) / 60);
        startDay = saveData.daysSurvived;
    }


    // Start is called before the first frame update
    void Start()
    {
        saveData = SaveSystem.Instance.saveData;
        if(!SaveSystem.Instance.newGame)
            Load();
        
        currentTime = new DateTime().Date + TimeSpan.FromHours(startHour) + TimeSpan.FromDays(startDay);
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        daysSurvived = (currentTime - new DateTime().Date).Days;

        if (showTime)
        {
            timeText.text = currentTime.ToString("HH:mm");
            dayText.text = "Day " + daysSurvived;
        }
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}