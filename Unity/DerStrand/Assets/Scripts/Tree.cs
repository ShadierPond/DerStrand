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
        //thisTree = this.gameObject
    }

    private void Update()
    {
        if(treeHealth <= 0 && isFallen == false)
        {
            Rigidbody rb = thisTree.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.forward, ForceMode.Impulse);
            StartCoroutine(destroyTree());
            isFallen = true;
        }
    }

    private IEnumerator destroyTree()
    {
        yield return new WaitForSeconds(10);
        
    }
}
