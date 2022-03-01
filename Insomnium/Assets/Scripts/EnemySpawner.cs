using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float maxSpawnDelay = 8;
    [SerializeField] float minSpawnDelay = 5;
    [SerializeField] bool isLooping = true;
    [SerializeField] List<GameObject> spawnList;


    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        do
        {
            //Debug.Log("Enemy Spawned");
            Instantiate(spawnList[Random.Range(0, spawnList.Count)], transform.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        } while (isLooping);
    }
}
