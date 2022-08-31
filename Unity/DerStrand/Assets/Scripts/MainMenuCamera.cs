using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Terrain target;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 cameraAngle;
    [SerializeField] private Vector3 cameraPosition;
    
    private void FixedUpdate()
    {
        mainCamera.transform.RotateAround(target.terrainData.bounds.center, Vector3.up, cameraSpeed * Time.deltaTime);
        mainCamera.transform.LookAt(target.terrainData.bounds.center + cameraAngle);
    }
}
