using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRound : MonoBehaviour
{
    [SerializeField]
    private int currentRound = 1;
    public int CurrentRound
    {
        get
        {
            return currentRound;
        }
        private set
        {
            currentRound = value;
            EventManager.Instance.Raise(EventID.RoundChange, new RoundChangeNotify(currentRound, currentRound == bossRound));
        }
    }

    [SerializeField]
    private int roundNumber = 5;

    [SerializeField]
    private int bossRound = 5;

    [SerializeField]
    private float duration = 10;

    [SerializeField]
    private float timer;
    public float Timer
    {
        get
        {
            return timer;
        }
        private set
        {
            timer = Mathf.Clamp(value, 0, duration);
            EventManager.Instance.Raise(EventID.TimerChange, new TimeChangeNotify(timer));

        }
    }

    private bool stopTimer = false;
    private Action<Notify> OnRetry, OnStartNextRound, OnPlayerDeath;

    private void Awake()
    {
        OnStartNextRound = thisNotify => StartNextRound();
        OnPlayerDeath = thisNotify => stopTimer = true;
        OnRetry = thisNotify => Init();

        Init();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.StartNextRound, OnStartNextRound);
        EventManager.Instance.AddListener(EventID.PlayerDeath, OnPlayerDeath);
        EventManager.Instance.AddListener(EventID.StartGame, OnRetry);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.StartNextRound, OnStartNextRound);
        EventManager.Instance.RemoveListener(EventID.GameOver, OnPlayerDeath);
        EventManager.Instance.RemoveListener(EventID.StartGame, OnRetry);
    }

    void Update()
    {
        RoundUpdate();
    }

    private void Init()
    {
        CurrentRound = 1;
        Time.timeScale = 1f;
        Timer = duration;
        stopTimer = false;

        ResetEnemyStats();
    }

    private void RoundUpdate()
    {
        if (!stopTimer)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer <= 0f && !stopTimer)
        {
            stopTimer = true;
            GameManager.Instance.RoundEnd();
        }
    }

    public void StartNextRound()
    {
        CurrentRound++;
        EnemyPowerUp();
        Time.timeScale = 1f;
        if (CurrentRound != bossRound)
        {
            Timer = duration;
            stopTimer = false;
        }
        else
        {
            stopTimer = true;
        }
    }

    [SerializeField]
    private List<EnemyStats> enemiesStats;

    private void EnemyPowerUp()
    {
        foreach (var enemyStats in enemiesStats)
        {
            enemyStats.StatsGrowth();
            if (currentRound == bossRound)
            {
                enemyStats.StopDropFragment();
            }
        }
    }

    private void ResetEnemyStats()
    {
        foreach (var enemyStats in enemiesStats)
        {
            enemyStats.Init();
        }
    }
}
