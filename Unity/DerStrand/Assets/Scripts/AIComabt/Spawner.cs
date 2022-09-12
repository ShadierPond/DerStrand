using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }
    [SerializeField] GameObject[] spawners;
    [SerializeField] GameObject enemy;

    [SerializeField] int groupSize = 5;
    public List<GameObject> enemyGroup = new List<GameObject>();

    [SerializeField] float spawnTime = 1f;
    float timer;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        spawners = new GameObject[5];


        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

    }

    private void Update()
    {
        if (enemyGroup.Count < groupSize)
        {
            timer += Time.deltaTime;
            if (timer >= spawnTime)
            {
                SpawnEnemy();
                timer = 0;
            }
        }   
    }

    void SpawnEnemy()
    {
        int spawnerID = Random.Range(0, spawners.Length);
        var obj = Instantiate(enemy, spawners[spawnerID].transform.position, spawners[spawnerID].transform.rotation);
        enemyGroup.Add(obj);
    }

}
