using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombie;

    public void SpawnEnemy()
    {
        Instantiate(zombie, transform.position, Quaternion.identity);
        EnemyCounter.AddEnemy();
    }
}
