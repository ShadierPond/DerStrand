using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKi : MonoBehaviour
{
    public NavMeshAgent agent;                      // Navmeshagent ( für Orientierung des Gegners)
    public Transform player;                        // Spieler ( für Reaktion des Gegners auf Spieler )
    public LayerMask whatIsGround, whatIsPlayer;    // Boden ( zum laufen ), Spieler 

    [SerializeField] private PlayerProperties propManager;          // Playerproperty Manager
    [SerializeField] int playerDamage = 20;        // Verursachter Schaden am Spieler

    [Header("Patroullie")]
    [SerializeField] Vector3 walkPoint;             // Wegpunkt für Patroullie
    bool walkPoinSet;                               // Bool für Wegpunkt gesetzt
    [SerializeField] float walkPointRange;          // Distanz zwischen aktueller Position und Zielposition

    [Header("Angriff")]
    [SerializeField] float timeBetweenAttacks;      // Zeit zwischen Angriffen
    bool alreadyAttacked;                           // Bool für Angriffscooldown

    [Header("Wahrnehmung")]
    [SerializeField] float sightRange, attackRange;                 // Sichtweite, Angriffsreichweite
    [SerializeField] bool playerInSightRange, playerInAttackRange;  // Bool für Spieler in Sicht und Spieler in Angriffsreichweite

    private void Awake()
    {
        player = GameObject.Find("Player").transform;               // Position des Spielers
        agent = GetComponent<NavMeshAgent>();                       // Beziehen des Navmeshagents
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);     // Ermitteln ob Spieler in Sichtweite
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);   // Ermitteln ob Spieler in Angriffsreichweite

        if (!playerInSightRange && !playerInAttackRange) Patroling();   // Wenn Spieler NICHT in Sichtweite und NICHT in Angriffsreichweite -> Patroullie
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();  // Wenn Spieler in Sichtweite und NICHT in Angriffsreichweite -> Verfolgen
        if (playerInSightRange && playerInAttackRange) AttackPlayer();  // Wenn Spieler in Sichtweite UND in Angriffsreichweite -> Angreifen
    }

    private void Patroling()
    {
        if (!walkPoinSet) SearchWalkPoint();                            // Wenn kein Wegpunkt gesetzt -> suche Wegpunkt

        if (walkPoinSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;   // Gehe zum Wegpunkt

        if (distanceToWalkPoint.magnitude < 1f)                         // Wenn Entfernung unter 1f, setze Wegpunkt gesetzt auf falsch ( zum neusuchen )
            walkPoinSet = false;
    }

    private void SearchWalkPoint()                                      // Suche Wegpunkt
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);  // Zufällige Distanz auf der Z-Achse
        float randomX = Random.Range(-walkPointRange, walkPointRange);  // Zufällige Distanz auf der X-Achse

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z +randomZ); // Wegpunktberechnung

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))    // Abfrage ob gesetzter Wegpunkt sich auf festem Boden befindet (whatIsGround)
            walkPoinSet = true;                                         // Wegpunkt gefunden -> Boole walkPpointSet = true
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);                          // Ändere Zielrichtung (Wegpunkt) zu Spielerposition
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);                  // Zielrichtung ist Spieler
        transform.LookAt(player);                                  // Rotation um Spieler anzusehen ( für Angriff )

        if (!alreadyAttacked)
        {

            propManager.DealDamage(playerDamage);

            alreadyAttacked = true;                             // Start Angriffscooldown
            Invoke(nameof(ResetAttack), timeBetweenAttacks);    
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;         // Einstellung wieder angreifen
    }

}
