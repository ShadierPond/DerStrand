using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }            // for class acessability from outside
    [SerializeField] GameObject[] spawners;                         // list of spawnpoints
    [SerializeField] GameObject enemy;                              // what should be spawned ( kind of enemy )

    [SerializeField] int groupSize = 5;                             // maiximum groupsize
    public List<GameObject> enemyGroup = new List<GameObject>();    // list fo counting og spawned objects

    [SerializeField] float spawnTime = 1f;                          // time distance between spawns
    float timer;                                                    // passed time

    private void Awake()
    {
        Instance = this;                                            // connection of the class with the instance
    }


    void Start()
    {
        spawners = new GameObject[5];


        for (int i = 0; i < spawners.Length; i++)                   // as long as spawnerlist isnt filled ...
        {
            spawners[i] = transform.GetChild(i).gameObject;         // ... objects are created
        }

    }

    private void Update()
    {
        if (enemyGroup.Count < groupSize)                           // as long as enemy count is lower then groupsize
        {
            timer += Time.deltaTime;
            if (timer >= spawnTime)                                 // if passed time is larger then spawntime ...
            {
                SpawnEnemy();                                       // spawning enemy
                timer = 0;                                          // reset the timer
            }
        }   
    }

    void SpawnEnemy()                                               
    {
        int spawnerID = Random.Range(0, spawners.Length);           // random position in the possible spawnpossitions
        var obj = Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation); // spawn enemy
        enemyGroup.Add(obj);                                        // list enemygroup gets filled with an entry ( important for spawnlimit )
    }

}
