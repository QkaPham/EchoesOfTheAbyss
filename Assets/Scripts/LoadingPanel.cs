using DG.Tweening;
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

    [SerializeField]
    private Slider loadingSlider;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public float fadeValue;
    public float fadeTime;

    private bool startFlickering;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0f;
    }

    public void LoadScene(string scene)
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(Load(scene));
    }

    private IEnumerator Load(string scene)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingText.text = $"Loading... {asyncOperation.progress}%";
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingText.text = $"Press any key to continue";
                if (!startFlickering)
                {
                    startFlickering = true;
                    FlickeringLoadingText();
                }
                if (Input.anyKey)
                {
                    canvasGroup.alpha = 0f;
                    UIManager.Instance.Fade(1f, 1f, () =>
                    {
                        asyncOperation.allowSceneActivation = true;
                        UIManager.Instance.Fade(0f, 1f, null);
                    }
                    );
                }
            }
            yield return null;
        }
    }

    private void FlickeringLoadingText()
    {
        loadingText.DOFade(fadeValue, fadeTime).SetLoops(-1, LoopType.Yoyo);
    }
}
