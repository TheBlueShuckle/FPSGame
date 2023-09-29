using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombie;

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
            GameObject newEnemy = Instantiate(zombie, new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f)), Quaternion.identity);
            zombieSpawnTime = 0;
        }
    }
}
