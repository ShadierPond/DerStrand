using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private int spawnLimit;
    [SerializeField] private List<GameObject> fishes;
    private float timer;
    private void Update()
    {
        var fishCount = gameObject.transform.childCount;
        timer += Time.deltaTime;
        if (timer >= spawnTime && fishCount <= spawnLimit)
        {
            var fish = Instantiate(fishPrefab, gameObject.transform);
            timer = 0;
        }
    }
}
