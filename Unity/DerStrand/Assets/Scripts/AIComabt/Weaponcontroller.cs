using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponcontroller : MonoBehaviour
{
    [SerializeField] GameObject Spear;      // Einf�gbares Objekt ( in dem Fall Speer )
    bool CanAttack = true;                  // Bool, kann angreifen
    [SerializeField] float AttackCooldown = 1f;       // Zeit zwischen Angriffen, Im Inspector einstellbar

    void Update()
    {
        if (Input.GetMouseButtonDown(0))    // Wenn Maus gedr�ckt und Bool wahr -> Ausf�hren Angriff
        {
            if (CanAttack)                  // Wenn Angriffscooldown abgelaufen ...
            {
                CCAttack();                 // ... Ausf�hrung des Angriffs
            }
        }
    }

    public void CCAttack()                              // Nahkampfangriff ( Closecombatattack)
    {
        CanAttack = false;
        Animator anim = Spear.GetComponent<Animator>(); // Einf�gen der Animation
        anim.SetTrigger("Attack");
        StartCoroutine(RestAttackCooldown());           // Start Aktivierung Angriffscooldown
    }

    IEnumerator RestAttackCooldown()                    // Wartezeit zwischen einzelnen Angriffen
    {
        yield return new WaitForSeconds(AttackCooldown); 
        CanAttack = true;
    }

}
