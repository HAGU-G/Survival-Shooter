using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class EnemyInfo
{
    public Enemy enemy;
    public int weight;
    public int maxCount;
}

public class MobSpawner : MonoBehaviour
{
    private List<ObjectPool<Enemy>> objectPools = new();

    public EnemyInfo[] enemyList;
    private List<int> spawnTable = new();

    public Transform[] spawnPoints;
    private float spawnTimer;
    public float spawnInterval = 1f;

    private void Awake()
    {
        spawnTimer = spawnInterval;
    }

    private void Start()
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            int index = i;
            objectPools.Add(
                new ObjectPool<Enemy>(
                    () =>
                    {
                        Enemy enemy = Instantiate(enemyList[index].enemy);
                        SetPool(enemy, index);
                        return enemy;
                    },
                    OnTakeFromPool,
                    OnReturnToPool,
                    OnDestroyPoolObject,
                    true, enemyList[index].maxCount / 5 + 1));
            for (int j = 0; j < enemyList[i].weight; j++)
                spawnTable.Add(i);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            int poolIndex = spawnTable[Random.Range(0, spawnTable.Count)];
            if (objectPools[poolIndex].CountActive < enemyList[poolIndex].maxCount)
            {
                objectPools[poolIndex].Get();
                spawnTimer = 0f;
            }
        }
    }

    private void SetPool(Enemy enemy, int index)
    {
        enemy.pool = objectPools[index];
    }

    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        enemy.transform.SetPositionAndRotation(spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    private void OnReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Enemy enemy)
    {
        Destroy(enemy);
    }
}
