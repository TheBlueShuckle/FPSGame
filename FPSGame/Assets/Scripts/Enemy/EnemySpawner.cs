using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombie;
    [SerializeField] private float spawnSpread = 5f;

    private float zombieSpawnTime = 0;
    private float zombieSpawnTimeMax = 2.5f;

    private void Update()
    {
        if (GameObject.FindWithTag("Enemy") == null)
        {
            zombieSpawnTime += Time.deltaTime;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if(zombieSpawnTime >= zombieSpawnTimeMax)
        {
            GameObject newEnemy = Instantiate(zombie, RandomSpawnLocation(), Quaternion.identity);
            zombieSpawnTime = 0;
        }
    }

    private Vector3 RandomSpawnLocation()
    {
        return new Vector3(transform.position.x + Random.Range(-spawnSpread, spawnSpread), 1, transform.position.y + Random.Range(-spawnSpread, spawnSpread));
    }
}
