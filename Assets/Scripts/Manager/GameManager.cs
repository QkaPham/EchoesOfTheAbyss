using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public static event Action OnStartGame;
    public static event Action OnRoundEnd;
    public static event Action OnStartNextRound;
    public static event Action OnRetryGame;
    public static event Action OnGameOver;
    public static event Action OnVictory;

    private void Update()
    {
        if (InputManager.Instance.Cancel)
        {
            if (!UIManager.Instance.GamePanel.isActive) return;
            if (UIManager.Instance.UpgradePanel.isActive) return;
            if (UIManager.Instance.SettingsPanel.isActive) return;

            if (UIManager.Instance.PausePanel.isActive)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UIManager.Instance.MainMenuPanel.Activate(false);
        UIManager.Instance.GamePanel.Activate(true);
        InputManager.instance.EnablePlayerInput(true);
        OnStartGame?.Invoke();
    }

    public void Pause()
    {
        StartCoroutine(DelayPause());
    }
    private IEnumerator DelayPause()
    {
        Time.timeScale = 0f;
        UIManager.Instance.PausePanel.Activate(true);
        InputManager.Instance.EnablePlayerInput(false);
        UIManager.Instance.ActiveDepthOfField(true);
        yield return null;
        UIManager.Instance.PausePanel.isActive = true;
    }
    public void Resume()
    {
        StartCoroutine(DelayResume());
    }

    private IEnumerator DelayResume()
    {
        Time.timeScale = 1f;
        UIManager.Instance.PausePanel.Activate(false);
        InputManager.Instance.EnablePlayerInput(true);
        UIManager.Instance.ActiveDepthOfField(false);
        yield return null;
        UIManager.Instance.PausePanel.isActive = false;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        UIManager.Instance.GamePanel.Activate(true);
        InputManager.Instance.EnablePlayerInput(true);
        OnStartGame?.Invoke();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        UIManager.Instance.MainMenuPanel.Activate(true);
        UIManager.Instance.PausePanel.Activate(false);
        UIManager.Instance.GamePanel.Activate(false);
        UIManager.Instance.ActiveDepthOfField(false);
        InputManager.Instance.EnablePlayerInput(false);
    }

    public void RoundEnd()
    {
        UIManager.Instance.UpgradePanel.Activate(true);
        InputManager.Instance.EnablePlayerInput(false);
        OnRoundEnd?.Invoke();
    }

    public void StartNextRound()
    {
        UIManager.Instance.UpgradePanel.Activate(false);
        InputManager.Instance.EnablePlayerInput(true);
        OnStartNextRound?.Invoke();
    }

    public void GameOver()
    {
        //Time.timeScale = 0f;
        UIManager.Instance.GamePanel.Activate(false);
        UIManager.Instance.GameoverPanel.Activate(true);
        InputManager.Instance.EnablePlayerInput(false);
        OnGameOver?.Invoke();
    }

    public void Victory()
    {
        UIManager.Instance.GamePanel.Activate(false);
        UIManager.Instance.VictoryPanel.Activate(true);
        InputManager.Instance.EnablePlayerInput(false);
        OnVictory?.Invoke();
    }

    public void Quit()
    {
        //TODO: Save
        Application.Quit();
    }
}
