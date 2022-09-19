using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeKi : MonoBehaviour
{
    private NavMeshAgent _agent;                    // Navmeshagent ( for orientation )
    public GameObject Player;                       // player ( to know form who to flee )

    [SerializeField] float FleeDistance = 4.0f;     // flee distance ( how far )

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();          
        Player = GameObject.Find("Player").gameObject;  
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);    // calculating distance between enemy and player

        if (distance < FleeDistance)                                                // if distance is smaller then flee distance ...
        {
            Vector3 dirToPlayer = transform.position - Player.transform.position;   // ... creating new movementdirection ...
            Vector3 newPos = transform.position + dirToPlayer;                      // ... thats farther away from player

            _agent.SetDestination(newPos);                                          // setting of new movementdestination
        }
    }


}
