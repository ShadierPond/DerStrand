using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    [SerializeField] GameObject[] spawners;                         // Liste der Spawnpunkte
    [SerializeField] GameObject enemy;                              // Zu spawnendes Objekt, im Inspector einfügbar

    [SerializeField] int groupSize = 5;                             // maximale Gruppengröße der Gegner
    public List<GameObject> enemyGroup = new List<GameObject>();    // Liste zum Zählen der gespawnten Gegner

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
        if (enemyGroup.Count < groupSize)                           // wird ausgeführt solange Gegnerzahl kleiner als (maximale) Gruppengröße
        {
            timer += Time.deltaTime;
            if (timer >= spawnTime)                                 // Wenn vergangene Zeit größer als Spawnzeit ...
            {
                SpawnEnemy();                                       // Spawnen von Gegner
                timer = 0;                                          // Reset des Timers
            }
        }   
    }

    void SpawnEnemy()                                               // Spawnen des (im Inspector) voreingestellten Gegners
    {
        int spawnerID = Random.Range(0, spawners.Length);           // Zufällige Position innerhalb der Spawnerpositionen
        var obj = Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
        enemyGroup.Add(obj);                                        // Liste enemyGroup wird mit Eintrag gefüllt ( wichtig für Spawnlimit)
    }

}
