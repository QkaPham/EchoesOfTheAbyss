using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private SceneLoader sceneLoader;
    public void StartGame()
    {
        sceneLoader.LoadScene("Game", () =>
        {
            InputManager.Instance.EnablePlayerInput(true);
            EventManager.Instance.Raise(EventID.StartGame, null);
            UIManager.Instance.Show(View.Game, null, false);
            SceneManager.UnloadSceneAsync("MainMenu");
        });
    }

    public void Pause()
    {
        UIManager.Instance.Show(View.Pause);
        InputManager.Instance.EnablePlayerInput(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        UIManager.Instance.ShowLast(() =>
        {
            Time.timeScale = 1f;
            InputManager.Instance.EnablePlayerInput(true);
        });
    }

    public void RetryGame()
    {
        UIManager.Instance.ShowLast();
        InputManager.Instance.EnablePlayerInput(true);
        EventManager.Instance.Raise(EventID.Retry, null);
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        sceneLoader.LoadScene("MainMenu", () =>
        {
            InputManager.Instance.EnablePlayerInput(false);
            UIManager.Instance.Show(View.MainMenu, null, false);
            UIManager.Instance.ActiveDepthOfField(false);
            SceneManager.UnloadSceneAsync("Game");
        });
    }

    public void RoundEnd()
    {
        UIManager.Instance.Show(View.Upgrade);
        InputManager.Instance.EnablePlayerInput(false);
        EventManager.Instance.Raise(EventID.RoundEnd, null);
    }

    public void StartNextRound()
    {
        UIManager.Instance.ShowLast();
        EventManager.Instance.Raise(EventID.StartNextRound, null);
        InputManager.Instance.EnablePlayerInput(true);
    }

    public void GameOver()
    {
        UIManager.Instance.Show(View.GameOver);
        EventManager.Instance.Raise(EventID.GameOver, null);
        InputManager.Instance.EnablePlayerInput(false);
    }

    public void Victory()
    {
        UIManager.Instance.Show(View.Victory);
        EventManager.Instance.Raise(EventID.Victory, null);
        InputManager.Instance.EnablePlayerInput(false);
    }

    public void Quit()
    {
        //TODO: Save game process
        Application.Quit();
    }
}
