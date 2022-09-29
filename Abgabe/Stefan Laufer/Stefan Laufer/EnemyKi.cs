using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKi : MonoBehaviour
{
    public NavMeshAgent agent;                      // for orientation for the enemy
    public Transform player;                        // player ( for reaction to player )
    public LayerMask whatIsGround, whatIsPlayer;    // ground ( for walking and orientation ), player 

    [SerializeField] private PlayerProperties propManager;          // Playerproperty Manager
    [SerializeField] int playerDamage = 20;        // dealt damage

    [Header("Patroullie")]
    [SerializeField] Vector3 walkPoint;             // waypoint for patrol
    bool walkPoinSet;                               // bool for waypoint set
    [SerializeField] float walkPointRange;          // distance between actual position and calculated position

    [Header("Angriff")]
    [SerializeField] float timeBetweenAttacks;      // time betweeen attacks
    bool alreadyAttacked;                           // bool for already attacked

    [Header("Wahrnehmung")]
    [SerializeField] float sightRange, attackRange;                 // sight range ( how far can see), atatck range ( when in meleerange)
    [SerializeField] bool playerInSightRange, playerInAttackRange;  // bool for in sightrange and in attackrange

    private void Awake()
    {
        player = GameObject.Find("Player").transform;               // position of player
        agent = GetComponent<NavMeshAgent>();
        propManager = PlayerProperties.Instance;
    }



    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);     // calculation if player in sightrange
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);   // calculation if player in attackrange

        if (!playerInSightRange && !playerInAttackRange) Patroling();   // if player NOT in sightrange and NOT in attackrange -> patrol
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();  // if player in sightrange and NOT in attackrange -> chase
        if (playerInSightRange && playerInAttackRange) AttackPlayer();  // if player in sightrange AND in attackrange -> attack
    }

    private void Patroling()
    {
        if (!walkPoinSet) SearchWalkPoint();                            // if no waypoint is set -> search for waypoint

        if (walkPoinSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;  

        if (distanceToWalkPoint.magnitude < 1f)                         // if distance is below 1f, set walkpoint set to false
            walkPoinSet = false;
    }

    private void SearchWalkPoint()                                      
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);  // random distance on z-axis
        float randomX = Random.Range(-walkPointRange, walkPointRange);  // random distance on x-axis

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z +randomZ); // waypoint calculation

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))    // control if set waypoint is acessable ground (whatIsGround)
            walkPoinSet = true;                                         
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);                          // change destination to playerposition
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);                  
        transform.LookAt(player);                                  // rotate to look at player ( for attack )

        if (!alreadyAttacked)
        {

            propManager.DealDamage(playerDamage);

            alreadyAttacked = true;                             // start attack cooldown
            Invoke(nameof(ResetAttack), timeBetweenAttacks);    
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;         // can attack again
    }

}
