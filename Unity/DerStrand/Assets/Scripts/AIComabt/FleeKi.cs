using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeKi : MonoBehaviour
{
    private NavMeshAgent _agent;
    public GameObject Player;

    public float FleeDistance = 4.0f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Player").gameObject;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance < FleeDistance)
        {
            Vector3 dirToPlayer = transform.position - Player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;

            _agent.SetDestination(newPos);
        }
    }


}
