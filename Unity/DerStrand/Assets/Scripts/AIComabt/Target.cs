using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    [SerializeField] float health;              // lifepoints ( for enemy, in inspector adjustable )
    
    [SerializeField] Item[] itemsDrops;              // array of items that can be dropped ( in inspector )
    [SerializeField] int[] itemsDropsAmounts;             // array of dropchances ( in inspector )
    [SerializeField] float[] dropChance;             // array of dropchances ( in inspector )
    

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
        for (int i = 0; i < itemsDrops.Length; i++)
        {
            if(Random.Range(0, 100) <= dropChance[i])
            {
                Player.Instance.inventory.AddItem(itemsDrops[i], itemsDropsAmounts[i]);
            }
        }
        Destroy(gameObject);                             // destruction of the instanciated object
    }
}
