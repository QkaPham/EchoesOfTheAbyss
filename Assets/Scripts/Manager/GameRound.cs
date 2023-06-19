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
            EventManager.Raise(EventID.RoundChange, new RoundChangeNotify(currentRound, currentRound == roundNumber));
        }
    }

    [SerializeField]
    private int roundNumber = 5;

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
            EventManager.Raise(EventID.TimerChange, new TimeChangeNotify(timer));

        }
    }

    private bool stopTimer = false;
    private Action<Notify> OnStartGame, OnStartNextRound, OnGameOver;

    private void Awake()
    {
        OnStartNextRound = thisNotify => StartNextRound();
        OnGameOver = thisNotify => stopTimer = true;

        Init();
    }
    private void Start()
    {
        EventManager.AddListener(EventID.StartNextRound, OnStartNextRound);
        EventManager.AddListener(EventID.GameOver, OnGameOver);
    }

    private void OnEnable()
    {
        //GameManager.OnStartGame += OnStartGame;
        //GameManager.OnStartNextRound += StartNextRound;
        // GameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        //GameManager.OnStartGame -= OnStartGame;
        //GameManager.OnStartNextRound -= StartNextRound;
        //GameManager.OnGameOver -= OnGameOver;
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
    }

    private void RoundUpdate()
    {
        if (CurrentRound != roundNumber && !stopTimer)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer <= 0f && !stopTimer)
        {
            stopTimer = true;
            EnemyPowerUp();
            GameManager.Instance.RoundEnd();
        }
    }

    public void StartNextRound()
    {
        CurrentRound++;
        Time.timeScale = 1f;
        Timer = duration;
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
