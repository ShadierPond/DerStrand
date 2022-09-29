using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUPDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    [SerializeField] private MenuAnimation menuAnim;

    private ObjectGrabbable objectGrabbable;    //grapabal object is called
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))   // is e is pressed 
        {
            if (objectGrabbable == null)    // and you do not have grabbed another item
            {   //Try to Grab
                float pickupDistance = 2f;  //set the pickup distance
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance, pickUpLayerMask))     //sends a raycast to detect the objectgrapabal
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))  //if the raycast hitted the grapabal object 
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);             //the Obvject get grapped
                        menuAnim.NotLookAtInteractable();                           //hide the E Button
                        //Debug.Log(objectGrabbable);                                 
                    }
                }
            }
            else
            {   //Currently holding something
                objectGrabbable.Drop();                                               //Item is dropped
                objectGrabbable = null;                                               //the Item container is set to null
                menuAnim.LookAtInteractable();                                        //the E Button gets enabled
            }
        }
    }
}
