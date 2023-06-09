using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject BossPrefabs;
    [SerializeField]
    private float bossIntroTime;

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
    private float enemySpawnTime = 1;
    private float lastSpawnTime = float.MinValue;

    public float delayOnStartRound;

    private bool isSpawning = true;

    [SerializeField]
    private CinemachineVirtualCamera vCam;

    private Action<Notify> StopSpawn, OnRoundChange;

    private void Awake()
    {
        StopSpawn = thisNotify => isSpawning = false;
        OnRoundChange = thisNotify =>
        {
            isSpawning = true;
            lastSpawnTime = Time.time + delayOnStartRound - enemySpawnTime;
            if (thisNotify is RoundChangeNotify notify)
            {
                if (notify.isBossRound)
                {
                    SpawnBoss();
                }
            }
        };
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.StartGame, OnRoundChange);
        EventManager.Instance.AddListener(EventID.RoundChange, OnRoundChange);

        EventManager.Instance.AddListener(EventID.RoundEnd, StopSpawn);
        EventManager.Instance.AddListener(EventID.Victory, StopSpawn);
        EventManager.Instance.AddListener(EventID.GameOver, StopSpawn);
    }
    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.StartGame, OnRoundChange);
        EventManager.Instance.RemoveListener(EventID.RoundChange, OnRoundChange);

        EventManager.Instance.RemoveListener(EventID.RoundEnd, StopSpawn);
        EventManager.Instance.RemoveListener(EventID.Victory, StopSpawn);
        EventManager.Instance.RemoveListener(EventID.GameOver, StopSpawn);
    }

    private void Update()
    {
        Spawn();
    }

    private void Spawn()
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
                BasePoolableEnemy enemy = enemyType.enemyPool.Get();
                enemy.Init(player, fragmentPool.pool, position);
                break;
            }
        }
    }

    public BossEnemy SpawnBoss()
    {
        GameObject gBoss = Instantiate(BossPrefabs, transform);
        BossEnemy bossEnemy = gBoss.GetComponent<BossEnemy>();
        Vector3 spawnPosition = player.transform.position.normalized * (-16) + player.transform.position;
        bossEnemy.Init(player, player.transform.position + spawnPosition, bulletPool);
        StartCoroutine(SetCameraFollow(bossEnemy));
        return bossEnemy;
    }

    private IEnumerator SetCameraFollow(BossEnemy bossEnemy)
    {
        vCam.Follow = bossEnemy.transform;
        yield return new WaitForSeconds(bossIntroTime);
        vCam.Follow = player.transform;
    }


    [Serializable]
    private class EnemyType
    {
        public BasePool<BasePoolableEnemy> enemyPool;
        public float spawnRate = 0.1f;
    }
}
