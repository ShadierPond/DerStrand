using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeKi : MonoBehaviour
{
    private NavMeshAgent _agent;                    // Navmeshagent ( zum Orientieren für den Gegner )
    public GameObject Player;                       // Spieler ( um zu wissen vor wem geflohen wird )

    [SerializeField] float FleeDistance = 4.0f;     // Fluchtdistanz ( wie weit geflohen wird ), einstellbar im Inspector

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();          // Beziehen des Navmeshagents
        Player = GameObject.Find("Player").gameObject;  // Beziehen des Gameobjects Spieler
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);    // Berechnung Abstand zwischen Gegner und Spieler

        if (distance < FleeDistance)                                                // Wenn errechneter Abstand kleiner als Fluchtdistanz ...
        {
            Vector3 dirToPlayer = transform.position - Player.transform.position;   // ... Erzeugen eines neuen Bewegungszielpunkts ...
            Vector3 newPos = transform.position + dirToPlayer;                      // ... der sich vom Spieler ENTFERNT

            _agent.SetDestination(newPos);                                          // Übergabe des neuen Wegzielpunkts
        }
    }


}
