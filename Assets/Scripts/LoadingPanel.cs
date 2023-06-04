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

    //[SerializeField]
    //private Slider loadingSlider;

    //[SerializeField]
    //private CanvasGroup canvasGroup;

    //public float fadeValue;
    //public float fadeTime;

    //private void Awake()
    //{
    //    canvasGroup = GetComponent<CanvasGroup>();
    //}

    //private void Start()
    //{
    //    canvasGroup.alpha = 0f;
    //}

    public void LoadScene(string scene)
    {
       // canvasGroup.alpha = 1f;
        StartCoroutine(Load(scene));
    }

    private IEnumerator Load(string scene)
    {
        Time.timeScale = 1f;
        UIManager.Instance.Fade(1f, 2f);
        float startTime = Time.time;
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {

            //loadingSlider.value = asyncOperation.progress;
            //loadingText.text = $"Loading... {asyncOperation.progress}%";
            if (asyncOperation.progress >= 0.9f)
            {
                //loadingSlider.value = 1f;
                //loadingText.text = $"Press any key to continue";
                if (Time.time >= startTime + 2f)
                {
                   // canvasGroup.alpha = 0f;
                    asyncOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}
