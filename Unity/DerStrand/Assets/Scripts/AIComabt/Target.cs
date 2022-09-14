using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float health;              // Lebenspunkte des Ziels (Target), im Inspector einstellbar

    public void TakeDamage(float amount)        // Berechnung Schaden bzw Tod
    {
        health -= amount;                       // Schadensberechnung
        if (health <= 0f)
        {
            Die();                              // Wenn LP kleiner gleich 0 -> Tod ( L�schen)
        }
    }

    void Die()
    {
        Spawner.Instance.enemyGroup.Remove(gameObject);  // L�sche Eintrag in der Spawnliste ( wichtig f�r Respawn)
        Destroy(gameObject);                             // Zerst�rung des instanzierten Objekts

    }
}
