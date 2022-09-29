using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    //Variables
    GameObject thisTree;
    public int treeHealth = 5;
    private bool isFallen = false;

    private void Start()
    {
        thisTree = transform.parent.gameObject;
        // or
        // thisTree = this.GameObject;
    }

    private void Update()
    {
        if (treeHealth <= 0 && isFallen == false) 
        //If Tree Health is less than or equal to 0 and "isFallen"(true/false statement) is equal to false...
        {
            Rigidbody rb = thisTree.AddComponent<Rigidbody>(); //New Variable is declared, Component is added to "thisTree".
            // The rigidbody is added here in runtime, because of the performance.
            // If every Tree has a static rigidbody -> very hard for CPU
            rb.isKinematic = false;
            rb.useGravity = true; //Gravity is activated
            rb.AddForce(Vector3.forward, ForceMode.Impulse); //For pushing forward
            StartCoroutine(destroyTree());
            isFallen = true; 
        }
    }

    private IEnumerator destroyTree() //add a Co-routine / Method
    {
        yield return new WaitForSeconds(10); //destroy the tree after 10 sec.
        Destroy(thisTree);
    }
}
