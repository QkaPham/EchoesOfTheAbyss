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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        sceneLoader.LoadScene("MainMenu", () =>
        {
            UIManager.Instance.Show(View.MainMenu);
            AudioManager.Instance.UnMuteSFX();
            AudioManager.Instance.PlayMusic("WhenInDoubt", 0, 4);
        });
    }


    public void StartGame()
    {
        AudioManager.Instance.FadeMusicVolume(0, 2);

        sceneLoader.LoadScene("Game", () =>
        {
            sceneLoader.UnloadScene("MainMenu");
            InputManager.Instance.EnablePlayerInput(true);
            EventManager.Instance.Raise(EventID.StartGame, null);
            UIManager.Instance.Show(View.Game, null, false);
            AudioManager.Instance.PlayMusic("BeforeItAllBegan", 2, 2);
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
        AudioManager.Instance.FadeMusicVolume(0, 2);
        sceneLoader.LoadScene("MainMenu", () =>
        {
            sceneLoader.UnloadScene("Game");
            InputManager.Instance.EnablePlayerInput(false);
            UIManager.Instance.Show(View.MainMenu, null, false);
            UIManager.Instance.ActiveDepthOfField(false);
            AudioManager.Instance.PlayMusic("WhenInDoubt", 2, 4);
        });
    }

    public void RoundEnd()
    {
        UIManager.Instance.Show(View.Upgrade, null, true, 1f);
        AudioManager.Instance.FadeMusicVolume(0.5f);
        InputManager.Instance.EnablePlayerInput(false);
        EventManager.Instance.Raise(EventID.RoundEnd, null);
    }

    public void StartNextRound()
    {
        AudioManager.Instance.FadeMusicVolume(1f);
        UIManager.Instance.ShowLast();
        EventManager.Instance.Raise(EventID.StartNextRound, null);
        InputManager.Instance.EnablePlayerInput(true);
    }

    public void GameOver()
    {
        UIManager.Instance.Show(View.GameOver, null, true, 1f);
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
