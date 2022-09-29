using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius;
    [SerializeField] private Item[] itemsToSpawn;
    [SerializeField] private int[] spawnLimits;
    [SerializeField] private int[] spawnChance;

    private void Start()
    {
        SpawnItems();
    }

    private void SpawnItems()
    {
        for (var i = 0; i < itemsToSpawn.Length; i++)
        {
            for (var j = 0; j < spawnLimits[i]; j++)
            {
                if (Random.Range(0, 100) >= spawnChance[i]) 
                    continue;
                var itemToSpawn = Instantiate(itemsToSpawn[i].prefab, transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity, transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
