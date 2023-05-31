using System;
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

    private bool isRoundEnded = false;

    public static event Action<float> OnTimerUpdate;
    // public static event Action OnRoundEnd;

    [SerializeField]
    private EnemySpawn enemySpawn;

    private void Start()
    {
        roundTimer = roundDuration;
    }
    private void OnEnable()
    {
        GameManager.OnStartGame += OnStartGame;
        GameManager.OnStartNextRound += StartNextRound;
    }

    private void OnDisable()
    {
        GameManager.OnStartGame -= OnStartGame;
        GameManager.OnStartNextRound -= StartNextRound;
    }

    void Update()
    {
        RoundUpdate();
    }
    private void OnStartGame()
    {
        currentRound = 1;
        Time.timeScale = 1f;
        roundTimer = roundDuration;
        isRoundEnded = false;
    }
    private void RoundUpdate()
    {
        if (currentRound != roundNumber)
        {
            roundTimer -= Time.deltaTime;
        }
        OnTimerUpdate?.Invoke(roundTimer);

        if (roundTimer <= 0f && !isRoundEnded)
        {
            isRoundEnded = true;
            GameManager.Instance.RoundEnd();
            //OnRoundEnd?.Invoke();
            //InputManager.Instance.EnablePlayerInput(false);
            //if (currentRound == roundNumber)
            //{
            //    //OnVictory?.Invoke();
            //    UIManager.Instance.VictoryPanel.Activate(true);
            //}
            //else
            //{

            //    UIManager.Instance.UpgradePanel.Activate(true);
            //}
        }
    }

    public void StartNextRound()
    {
        currentRound++;
        if (currentRound == roundNumber)
        {
            enemySpawn.SpawnBoss();
        }
        Time.timeScale = 1f;
        roundTimer = roundDuration;
        isRoundEnded = false;
    }
}
