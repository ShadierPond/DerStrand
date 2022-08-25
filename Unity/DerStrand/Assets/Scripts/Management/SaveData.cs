using UnityEngine;
[System.Serializable]
public class SaveData
{
    // Save Information
    [Header("Save Information")]
    public string saveDate;
    public string saveTime;
    public int daysSurvived;
    public string inGameTime;
    
    // Player Information
    [Header("Player Information")]
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector2 playerCameraRotation;
    
}


