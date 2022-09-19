using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponcontroller : MonoBehaviour
{
    [SerializeField] GameObject Spear;      // insertable objekt ( in this case spear )
    bool CanAttack = true;                  // Bool, can attack
    [SerializeField] float AttackCooldown = 1f;       // time between attack, 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))    // if left mouseclick and bool (can attack) true -> 
        {
            if (CanAttack)                  // if bool true ...
            {
                CCAttack();                 // ... start attack
            }
        }
    }

    public void CCAttack()                              // melee attack ( Closecombatattack)
    {
        CanAttack = false;
        Animator anim = Spear.GetComponent<Animator>(); // animation for spearattack
        anim.SetTrigger("Attack");
        StartCoroutine(RestAttackCooldown());           // starting attackcooldown
    }

    IEnumerator RestAttackCooldown()                    // waitintime between attacks
    {
        yield return new WaitForSeconds(AttackCooldown); 
        CanAttack = true;
    }

}
