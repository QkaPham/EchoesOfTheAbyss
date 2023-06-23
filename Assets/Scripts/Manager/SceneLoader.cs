using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Action TempAction;

    public void LoadScene(string scene, Action onSceneLoaded = null, bool showLoadScene = true)
    {
        TempAction = onSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        UIManager.Instance.ClearHistoryViews();
        if (showLoadScene)
        {
            UIManager.Instance.Show(View.Load, () =>
            {
                asyncOperation.allowSceneActivation = true;
            }, false);
        }
        else
        {
            asyncOperation.allowSceneActivation = true;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        TempAction?.Invoke();
    }
}
