using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Terrain target;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector3 cameraAngle;

    private void FixedUpdate()
    {
        mainCamera.transform.RotateAround(target.terrainData.bounds.center, target.transform.up, cameraSpeed * Time.deltaTime);
        mainCamera.transform.LookAt(target.terrainData.bounds.center + cameraAngle);
    }
}
