using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class RoundHandler : MonoBehaviour
{
    [SerializeField] private int spawnCap;
    [SerializeField] private RoundData roundData;
    [SerializeField] GameObject[] spawners;

    [SerializeField] int graceTimeMax;
    [SerializeField] int spawnTimerMax;

    public int CurrentRound { get => currentRound; }

    private int[] zombiesPerRound;
    private int[] zombiesPerSpawner = new int[0];

    private int currentRound = 0;
    private int currentSpawner = 0;

    private Timer graceTimer;
    private Timer spawnTimer;

    private void Update()
    {
        print(EnemyCounter.GetEnemyCount());

        graceTimer?.Update(Time.deltaTime);
        spawnTimer?.Update(Time.deltaTime);

        if (EnemyCounter.GetEnemyCount() <= 0 && zombiesPerSpawner.Sum() <= 0)
        {
            print("round won");

            graceTimer ??= new Timer(graceTimeMax);

            if (graceTimer.IsDone())
            {
                print("spawning zombies");
                currentRound++;
                CalculateZombiesPerSpawner(roundData.enemiesPerRound[currentRound]);

                currentSpawner = 0;
                graceTimer = null;
                spawnTimer = new Timer(0);
            }
        }

        if (spawnTimer != null)
        {
            if (zombiesPerSpawner.Sum() <= 0)
            {
                spawnTimer = null;
            }

            else if (spawnTimer.IsDone())
            {
                SpawnWaveOfZombies();
                spawnTimer = new Timer(spawnTimerMax);
            }
        }
    }

    private void SpawnWaveOfZombies()
    {
        for (int i = currentSpawner; i < spawners.Length; i++)
        {
            int indexOfSpawner = System.Array.IndexOf(spawners, spawners[i]);

            if (zombiesPerSpawner[indexOfSpawner] > 0 && EnemyCounter.GetEnemyCount() < spawnCap)
            {
                spawners[i].GetComponent<EnemySpawner>().SpawnEnemy();

                zombiesPerSpawner[indexOfSpawner]--;
            }

            else
            {
                currentSpawner = indexOfSpawner;
                return;
            }

            currentSpawner = 0;
        }
    }

    private void CalculateZombiesPerSpawner(int zombiesThisRound)
    {
        zombiesPerSpawner = new int[spawners.Length];
        int i = 0;

        while (zombiesThisRound > 0)
        {
            zombiesPerSpawner[i]++;
            i++;

            if (i > zombiesPerSpawner.Length - 1)
            {
                i = 0;
            }

            zombiesThisRound--;
        }
    }
}
