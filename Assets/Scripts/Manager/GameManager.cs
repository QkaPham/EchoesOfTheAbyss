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

    public void StartGame()
    {
        SceneManager.sceneLoaded += OnGameLevelLoaded;
        UIManager.Instance.LoadScene("GameLevel");
    }

    private void OnGameLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnGameLevelLoaded;
        UIManager.Instance.MainMenuPanel.Activate(false);
        UIManager.Instance.GamePanel.Activate(true);
        UIManager.Instance.Fade(0f, 1f);
        InputManager.instance.EnablePlayerInput(true);
        OnStartGame?.Invoke();
    }

    public void Pause()
    {
        UIManager.Instance.PausePanel.Activate(true);
        InputManager.Instance.EnablePlayerInput(false);
        UIManager.Instance.ActiveDepthOfField(true);
    }

    public void Resume()
    {
        UIManager.Instance.PausePanel.Activate(false);
        UIManager.Instance.ActiveDepthOfField(false);
        InputManager.Instance.EnablePlayerInput(true);
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
        SceneManager.sceneLoaded += OnMainmenuLoaded;
        UIManager.Instance.LoadScene("MainMenu");
    }

    private void OnMainmenuLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnMainmenuLoaded;
        UIManager.Instance.MainMenuPanel.Activate(true);
        UIManager.Instance.PausePanel.Activate(false);
        UIManager.Instance.GamePanel.Activate(false);
        UIManager.Instance.ActiveDepthOfField(false);
        UIManager.Instance.Fade(0f, 2f);
        InputManager.Instance.EnablePlayerInput(false);
    }


    public void RoundEnd()
    {
        UIManager.Instance.UpgradePanel.Activate(true);
        UIManager.Instance.GamePanel.Activate(false);
        InputManager.Instance.EnablePlayerInput(false);
        OnRoundEnd?.Invoke();
    }

    public void StartNextRound()
    {
        UIManager.Instance.UpgradePanel.Activate(false);
        UIManager.Instance.GamePanel.Activate(true);
        InputManager.Instance.EnablePlayerInput(true);
        OnStartNextRound?.Invoke();
    }

    public void GameOver()
    {
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
        //TODO: Save game process
        Application.Quit();
    }
}
