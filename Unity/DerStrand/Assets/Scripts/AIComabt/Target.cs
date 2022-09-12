using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float health;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Spawner.Instance.enemyGroup.Remove(gameObject);
        Destroy(gameObject);

    }
}
