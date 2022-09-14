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
            Die();                              // Wenn LP kleiner gleich 0 -> Tod ( Löschen)
        }
    }

    void Die()
    {
        Spawner.Instance.enemyGroup.Remove(gameObject);  // Lösche Eintrag in der Spawnliste ( wichtig für Respawn)
        Destroy(gameObject);                             // Zerstörung des instanzierten Objekts

    }
}
