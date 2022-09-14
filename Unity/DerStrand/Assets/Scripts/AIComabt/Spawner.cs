using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    [SerializeField] GameObject[] spawners;                         // Liste der Spawnpunkte
    [SerializeField] GameObject enemy;                              // Zu spawnendes Objekt, im Inspector einf�gbar

    [SerializeField] int groupSize = 5;                             // maximale Gruppengr��e der Gegner
    public List<GameObject> enemyGroup = new List<GameObject>();    // Liste zum Z�hlen der gespawnten Gegner

    [SerializeField] float spawnTime = 1f;                          // Zeitabstand zwischen den Spawns
    float timer;                                                    // Vergangene Zeit

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        spawners = new GameObject[5];


        for (int i = 0; i < spawners.Length; i++)                   // Solange die Spawnerliste nicht voll ist ...
        {
            spawners[i] = transform.GetChild(i).gameObject;         // ... werden Objekte erzeugt
        }

    }

    private void Update()
    {
        if (enemyGroup.Count < groupSize)                           // wird ausgef�hrt solange Gegnerzahl kleiner als (maximale) Gruppengr��e
        {
            timer += Time.deltaTime;
            if (timer >= spawnTime)                                 // Wenn vergangene Zeit gr��er als Spawnzeit ...
            {
                SpawnEnemy();                                       // Spawnen von Gegner
                timer = 0;                                          // Reset des Timers
            }
        }   
    }

    void SpawnEnemy()                                               // Spawnen des (im Inspector) voreingestellten Gegners
    {
        int spawnerID = Random.Range(0, spawners.Length);           // Zuf�llige Position innerhalb der Spawnerpositionen
        var obj = Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
        enemyGroup.Add(obj);                                        // Liste enemyGroup wird mit Eintrag gef�llt ( wichtig f�r Spawnlimit)
    }

}
