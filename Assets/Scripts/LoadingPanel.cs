using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI loadingText;
    protected LoadView view;
    protected Action onSceneLoaded;

    private void Awake()
    {
        view = GetComponent<LoadView>();
    }

    public void LoadScene(string scene, View viewName, Action onSceneLoaded = null)
    {
        this.onSceneLoaded = onSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;
        UIManager.Instance.Show(View.Load, () =>
        {
            asyncOperation.allowSceneActivation = true;
            UIManager.Instance.Show(viewName, null, false);
        }, false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        onSceneLoaded?.Invoke();
    }

    //private IEnumerator Load(string scene, View viewName, float minDuration = 1f)
    //{
    //    Time.timeScale = 1f;
    //    float startTime = Time.time;
    //    yield return null;
    //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
    //    asyncOperation.allowSceneActivation = false;
    //    UIManager.Instance.Show(View.Load, () =>
    //    {
    //        asyncOperation.allowSceneActivation = true;
    //        UIManager.Instance.Show(viewName, null, true);
    //    }, false);
    //}
}
