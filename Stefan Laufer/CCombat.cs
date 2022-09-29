using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCombat : MonoBehaviour
{
    [SerializeField] float damage = 10f;            // damage
    [SerializeField] float range = 20f;             // range
    [SerializeField] float fireRate = 15f;          // firerate
    [SerializeField] float impactForce = 300f;      // impact ( knockback )

    [SerializeField] Camera fpsCam;                 // used camera ( in inspector adjustable )

    private float nextTimeToFire = 0f;              // time to next attack

    void Update()
    {

        if (Player.Instance.input.actions["Fire"].triggered && Time.time >= nextTimeToFire) // if left mouse is clicked and attack cooldown expired
        {
            nextTimeToFire = Time.time + 1f / fireRate;  // attackcooldown is set
            SpAttack();                                     // (spear) attack
        }

    }

    void SpAttack()                                                              // spear attack
    {

        var frontObject = Player.Instance.GetRaycastObject();                   
               
            Target target = frontObject.transform.GetComponent<Target>();       // look if target hast a target script in it

            if (target != null)                                                 // if target possible ...
            {
                target.TakeDamage(damage);                                      // target takes damage
            }

            if (frontObject.GetComponent<Rigidbody>() != null)                  // if target has a rigidbody ...
            {
                frontObject.GetComponent<Rigidbody>().AddForce(-frontObject.transform.position * impactForce);  // apply impactforce to target
            }
                    
    }
}
