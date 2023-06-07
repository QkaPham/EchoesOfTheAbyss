using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundTimer : MonoBehaviour
{
    [SerializeField]
    private int currentRound;

    [SerializeField]
    private int roundNumber = 5;

    [SerializeField]
    private float roundDuration = 10;

    [SerializeField]
    private float roundTimer;

    private bool stopTimer = false;

    [SerializeField]
    private EnemySpawn enemySpawn;

    public static event Action BossRoundStart;

    private void Start()
    {
        roundTimer = roundDuration;
    }

    private void OnEnable()
    {
        GameManager.OnStartGame += OnStartGame;
        GameManager.OnStartNextRound += StartNextRound;
        GameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= OnStartGame;
        GameManager.OnStartNextRound -= StartNextRound;
        GameManager.OnGameOver -= OnGameOver;
    }

    void Update()
    {
        RoundUpdate();
    }

    private void OnGameOver()
    {
        stopTimer = true;
    }

    private void OnStartGame()
    {
        currentRound = 1;
        UIManager.Instance.GamePanel.UpdateRoundText(currentRound);
        Time.timeScale = 1f;
        roundTimer = roundDuration;
        stopTimer = false;
    }
    private void RoundUpdate()
    {
        if (currentRound != roundNumber && !stopTimer)
        {
            roundTimer -= Time.deltaTime;
            UIManager.Instance.GamePanel.UpdateRoundtimerText(roundTimer);
        }

        if (roundTimer <= 0f && !stopTimer)
        {
            stopTimer = true;
            EnemyPowerUp();
            GameManager.Instance.RoundEnd();
        }
    }

    public void StartNextRound()
    {
        currentRound++;
        UIManager.Instance.GamePanel.UpdateRoundText(currentRound);
        UIManager.Instance.UpgradePanel.UpdateGameProgress(currentRound);
        if (currentRound == roundNumber)
        {
            BossEnemy boss = enemySpawn.SpawnBoss();
            UIManager.Instance.GamePanel.UpdateRoundtimerText("");
            BossRoundStart?.Invoke();
        }
        Time.timeScale = 1f;
        roundTimer = roundDuration;
        stopTimer = false;
    }

    [SerializeField]
    private List<EnemyStats> enemiesStats;

    private void EnemyPowerUp()
    {
        foreach (var enemyStats in enemiesStats)
        {
            enemyStats.StatsGrowth();
        }
    }

}
