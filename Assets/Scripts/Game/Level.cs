using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{

    public LevelEnemySpawnData[] spawnData;
    public float minSpawnTime = 5.0f;
    public float maxSpawnTime = 10.0f;

    public int enemiesLeftToSpawn;
    public int enemiesAlive;
    public List<CharacterData> spawnPool;

    public Level(LevelEnemySpawnData[] spawnData, float minSpawnTime, float maxSpawnTime)
    {
        this.spawnData = spawnData;
        this.minSpawnTime = minSpawnTime;
        this.maxSpawnTime = maxSpawnTime;

        SetEnemiesLeftToSpawnCount();
        SetRandomSpawnPool();
    }

    public void SetEnemiesLeftToSpawnCount()
    {
        int count = 0;
        for (int i = 0; i < spawnData.Length; i++)
        {
            count += spawnData[i].count;
        }

        enemiesLeftToSpawn = count;
    }

    public CharacterData GetRandomEnemyToSpawn()
    {
        var enemy = spawnPool[UnityEngine.Random.Range(0, spawnPool.Count)];
        enemiesLeftToSpawn--;
        spawnPool.Remove(enemy);

        return enemy;
    }

    public float GetRandomSpawnCooldown()
    {
        return UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SetRandomSpawnPool()
    {
        List<CharacterData> templist = new List<CharacterData>();
        for (int i = 0; i < spawnData.Length; i++)
        {
            for (int j = 0; j < spawnData[i].count; j++)
            {
                templist.Add(spawnData[i].enemyData);
            }
        }

        templist.Shuffle();
        spawnPool = templist;
    }

}