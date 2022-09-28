using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();    //the rigidbody is searched after picking up the Item
    }
    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;   //the grab point is set
        objectRigidbody.useGravity = false;                         //the object cant fall anymore
    }
    public void Drop()
    {
        this.objectGrabPointTransform = null;                       //objectGrabPointTransform is set to null
        objectRigidbody.useGravity = true;                          //the gravity is activated agan
    }
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null) //if the item is graped
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed); // moves the object to a new position
            objectRigidbody.MovePosition(newPosition);  //gets the nest new position
        }
    }
}
