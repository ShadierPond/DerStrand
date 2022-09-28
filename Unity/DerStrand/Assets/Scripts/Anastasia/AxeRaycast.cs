using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeRaycast : MonoBehaviour
{
    //Variables
    public GameObject axe;
    private bool isEquipped = false;

    private void Update()
    {
        if (!axe.activeSelf && Input.GetKeyDown(KeyCode.Alpha1))
        {
            isEquipped = true;
            axe.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isEquipped = false;
            axe.SetActive(false);
        }

        //Raycast
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        //Origin, Direction, RaycastHit, Length
        if (Physics.Raycast(transform.position, fwd, out hit, 10))
        {
            if(hit.collider.tag == "tree" && Input.GetMouseButtonDown(0) && isEquipped == true)
            {
                //Tree treeScript = hit.collider.gameObject.GetComponent<Tree>();
                //treeScript.treeHealth--;
            }
        }
    }
}
