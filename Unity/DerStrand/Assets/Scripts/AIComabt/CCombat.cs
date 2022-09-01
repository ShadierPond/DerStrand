using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCombat : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] float range = 20f;
    [SerializeField] float fireRate = 15F;
    [SerializeField] float impactForce = 300f;

    [SerializeField] Camera fpsCam;

    private float nextTimeToFire = 0f;

    // Update is called once per frame
    void Update()
    {

        if (Player.Instance.input.actions["Fire"].triggered && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

    }

    void Shoot()
    {

        var frontObject = Player.Instance.GetRaycastObject();

       
        
            
            Target target = frontObject.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (frontObject.GetComponent<Rigidbody>() != null)
            {
                frontObject.GetComponent<Rigidbody>().AddForce(-frontObject.transform.position * impactForce);
            }

        
    }
}
