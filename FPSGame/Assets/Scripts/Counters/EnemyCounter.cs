using System.Collections;
using System.Collections.Generic;

public static class EnemyCounter
{
    static int enemies = 0;

    public static void AddEnemy()
    {
        enemies++;
    }

    public static void RemoveEnemy()
    {
        enemies--;
    }

    public static int GetEnemyCount()
    {
        return enemies;
    }
}
