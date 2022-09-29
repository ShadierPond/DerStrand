using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float health;              // lifepoints ( for enemy, in inspector adjustable )

    public void TakeDamage(float amount)        // claculation of damage ( death )
    {
        health -= amount;                       // damage calculation
        if (health <= 0f)
        {
            Die();                              // if health lower or equal 0 -> death (delete)
        }
    }

    void Die()
    {
        Spawner.Instance.enemyGroup.Remove(gameObject);  // clear entry in spawnlist( important for respawn )
        Destroy(gameObject);                             // destruction of the instanciated object

    }
}
