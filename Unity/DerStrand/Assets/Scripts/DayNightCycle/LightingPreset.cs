using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "DayNightCycle/Lighting Preset")]
public class LightingPreset : ScriptableObject
{
    // Light Ambient color
    public Gradient ambientColor;
    // Light Directional color
    public Gradient directionalColor;
    // Fog color
    public Gradient fogColor;
    
}
