using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject BossPrefabs;

    [SerializeField]
    private EnemyBulletPool bulletPool;

    [SerializeField]
    private List<EnemyType> enemyTypes;

    [SerializeField]
    private FragmentPool fragmentPool;

    [SerializeField]
    private ItemPool itemPool;

    [SerializeField]
    private float spawnDistance = 1.0f;

    [SerializeField]
    private float Offset = 1.0f;

    [SerializeField]
    private Camera mainCamera;

    private float lastSpawnTime = float.MinValue;
    [SerializeField] private float enemySpawnTime = 1;

    private bool isSpawning;

    [SerializeField]
    private CinemachineVirtualCamera vCam;

    private void OnEnable()
    {
        GameManager.OnStartGame += () => isSpawning = true;
        GameManager.OnRoundEnd += () => isSpawning = false;
        GameManager.OnStartNextRound += () => isSpawning = true;
        GameManager.OnGameOver += () => isSpawning = false;
        GameManager.OnVictory += () => isSpawning = false;
    }
    private void OnDisable()
    {
        GameManager.OnStartGame -= () => isSpawning = true;
        GameManager.OnRoundEnd -= () => isSpawning = false;
        GameManager.OnStartNextRound -= () => isSpawning = true;
        GameManager.OnGameOver -= () => isSpawning = false;
        GameManager.OnVictory -= () => isSpawning = false;
    }

    private void Update()
    {
        StartSpawn();
    }

    private void StartSpawn()
    {
        if (lastSpawnTime + enemySpawnTime <= Time.time && isSpawning)
        {
            SpawnObjectOutsideCameraView();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnObjectOutsideCameraView()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float x = UnityEngine.Random.Range(-spawnDistance, spawnDistance);
        float y = UnityEngine.Random.Range(-spawnDistance, spawnDistance);
        x += (x >= 0 ? topRight.x + Offset : bottomLeft.x - Offset);
        y += (y >= 0 ? topRight.y + Offset : bottomLeft.y - Offset);
        var spawnPosition = new Vector2(x, y);

        SpawnRandomEnemy(spawnPosition);
    }

    private void SpawnRandomEnemy(Vector3 position = default, Quaternion rotation = default)
    {
        float totalSpawnRate = 0f;
        foreach (var enemyType in enemyTypes)
        {
            totalSpawnRate += enemyType.spawnRate;
        }
        float randomValue = UnityEngine.Random.Range(0f, totalSpawnRate);

        float cumulativeSpawnRate = 0f;

        foreach (var enemyType in enemyTypes)
        {
            cumulativeSpawnRate += enemyType.spawnRate;

            if (randomValue <= cumulativeSpawnRate)
            {
                BasePoolableEnemy meleeEnemy = enemyType.enemyPool.Get();
                meleeEnemy.Init(player, fragmentPool.pool, position, itemPool.pool);
                break;
            }
        }
    }

    public BossEnemy SpawnBoss()
    {
        GameObject gBoss = Instantiate(BossPrefabs, transform);
        BossEnemy bossEnemy = gBoss.GetComponent<BossEnemy>();
        bossEnemy.Init(player, player.transform.position + Vector3.up * 16f, bulletPool);
        StartCoroutine(SetCameraFollow(bossEnemy));
        return bossEnemy;
    }


    private IEnumerator SetCameraFollow(BossEnemy bossEnemy)
    {
        vCam.Follow = bossEnemy.transform;
        yield return new WaitForSeconds(2);
        vCam.Follow = player.transform;
    }


    [Serializable]
    private class EnemyType
    {
        public BasePool<BasePoolableEnemy> enemyPool;
        public float spawnRate = 0.1f;
    }
}
