using System;
using UnityEngine;
[System.Serializable]
public class SaveData
{
    // Save Information
    [Header("Save Information")]
    public string saveDate;
    public string saveTime;
    public string inGameTime;
    
    // Player Information
    [Header("Player Information")]
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector2 playerCameraRotation;
    
    // DayCycle Information
    [Header("DayCycle Information")]
    public string startTime;
    public int daysSurvived;

}


