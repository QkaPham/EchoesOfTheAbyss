using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public void StartGame()
    {
        UIManager.Instance.LoadScene("GameLevel", View.Game, () =>
        {
            InputManager.Instance.EnablePlayerInput(true);
            EventManager.Raise(EventID.StartGame, null);
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
        EventManager.Raise(EventID.Retry, null);
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        UIManager.Instance.LoadScene("MainMenu", View.MainMenu, () =>
        {
            UIManager.Instance.ActiveDepthOfField(false);
            InputManager.Instance.EnablePlayerInput(false);
        });
    }

    public void RoundEnd()
    {
        UIManager.Instance.Show(View.Upgrade);
        InputManager.Instance.EnablePlayerInput(false);
        EventManager.Raise(EventID.RoundEnd, null);
    }

    public void StartNextRound()
    {
        UIManager.Instance.ShowLast();
        EventManager.Raise(EventID.StartNextRound, null);
        InputManager.Instance.EnablePlayerInput(true);
    }

    public void GameOver()
    {
        UIManager.Instance.Show(View.GameOver);
        EventManager.Raise(EventID.GameOver, null);
        InputManager.Instance.EnablePlayerInput(false);
    }

    public void Victory()
    {
        UIManager.Instance.Show(View.Victory);
        EventManager.Raise(EventID.Victory, null);
        InputManager.Instance.EnablePlayerInput(false);
    }

    public void Quit()
    {
        //TODO: Save game process
        Application.Quit();
    }
}
