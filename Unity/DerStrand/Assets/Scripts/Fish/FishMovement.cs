using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float speed;

    public void Update()
    {
        var parent = gameObject.transform.parent;
        var fishPosition = radius * Vector3.Normalize(this.transform.position + parent.transform.position);
        Debug.Log(fishPosition);
        Debug.Log(transform.position);
        transform.position = fishPosition;
        transform.RotateAround(parent.transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
