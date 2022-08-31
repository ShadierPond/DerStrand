using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUPDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;

    [SerializeField] private MenuAnimation menuAnim;

    private ObjectGrabbable objectGrabbable;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {   //Try to Grab
                float pickupDistance = 2f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance, pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                        menuAnim.NotLookAtInteractable();
                        Debug.Log(objectGrabbable);
                    }
                }
            }
            else
            {   //Currently holding something
                objectGrabbable.Drop();
                objectGrabbable = null;
                menuAnim.LookAtInteractable();
            }
        }
    }
}
