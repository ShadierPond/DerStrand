using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponcontroller : MonoBehaviour
{
    [SerializeField] GameObject Spear;
    bool CanAttack = true;
    public float AttackCooldown = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanAttack)
            {
                SwordAttack();
            }
        }
    }

    public void SwordAttack()
    {
        CanAttack = false;
        Animator anim = Spear.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        StartCoroutine(RestAttackCooldown());
    }

    IEnumerator RestAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

}
