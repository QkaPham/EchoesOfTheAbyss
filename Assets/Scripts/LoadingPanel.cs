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

    private void Awake()
    {
        view = GetComponent<LoadView>();
    }

    public void LoadScene(string scene, View viewName)
    {
        // canvasGroup.alpha = 1f;
        //StartCoroutine(Load(scene, viewName));
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;
        UIManager.Instance.Show(View.Load, () =>
        {
            asyncOperation.allowSceneActivation = true;
            UIManager.Instance.Show(viewName, null, true);
        }, false);
    }

    private IEnumerator Load(string scene, View viewName, float minDuration = 1f)
    {
        Time.timeScale = 1f;
        float startTime = Time.time;
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;
        UIManager.Instance.Show(View.Load, () =>
        {
            asyncOperation.allowSceneActivation = true;
            UIManager.Instance.Show(viewName, null, true);
        }, false);
        //view.Activate(minDuration, 0f, () =>
        //{

        //    view.DeActivate(minDuration, 0f, () =>
        //    {

        //    });
        //}
        //);

        //while (!asyncOperation.isDone)
        //{

        //    //loadingSlider.value = asyncOperation.progress;
        //    //loadingText.text = $"Loading... {asyncOperation.progress}%";
        //    if (asyncOperation.progress >= 0.9f)
        //    {
        //        //loadingSlider.value = 1f;
        //        //loadingText.text = $"Press any key to continue";
        //        if (Time.time >= startTime + minDuration)
        //        {
        //            // canvasGroup.alpha = 0f;
        //            asyncOperation.allowSceneActivation = true;
        //        }
        //    }
        //    yield return null;
        //}
    }
}
