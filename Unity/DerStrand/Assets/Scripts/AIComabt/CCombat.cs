using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCombat : MonoBehaviour
{
    [SerializeField] float damage = 10f;            // Verursachter Schaden
    [SerializeField] float range = 20f;             // Reichweite
    [SerializeField] float fireRate = 15f;          // Feuerrate
    [SerializeField] float impactForce = 300f;      // Einschlagskraft ( Rückschlag )

    [SerializeField] Camera fpsCam;                 // Genutzte Kamera ( im Inspector zuweisbar )

    private float nextTimeToFire = 0f;              // Zeit bis zur nächsten Angriffsauslösbarkeit

    void Update()
    {

        if (Player.Instance.input.actions["Fire"].triggered && Time.time >= nextTimeToFire) // Wenn linke Maustaste gedrückt UND Angriffscooldown abgelaufen
        {
            nextTimeToFire = Time.time + 1f / fireRate;  // Angriffscooldown wird gesetzt
            SpAttack();                                     // Auslösen des Angriffs
        }

    }

    void SpAttack()                                                              // Speerangriff
    {

        var frontObject = Player.Instance.GetRaycastObject();                   
               
            Target target = frontObject.transform.GetComponent<Target>();       // Schaut ob das Ziel die Komponente (Script) Target enthält

            if (target != null)                                                 // Wenn Ziel vorhanden ...
            {
                target.TakeDamage(damage);                                      // Ziel nimmt Schaden
            }

            if (frontObject.GetComponent<Rigidbody>() != null)                  // Wenn das Ziel einenen Rigidbody hat ...
            {
                frontObject.GetComponent<Rigidbody>().AddForce(-frontObject.transform.position * impactForce);  // Einschlagskraft (Rückstoß) auf das Ziel wirken
            }
                    
    }
}
