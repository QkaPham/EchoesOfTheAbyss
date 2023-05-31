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

    public void Activate()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GameLevel");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingText.text = $"Loading Scene: {asyncOperation.progress}%";
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingText.text = $"Press the spacbar to continue";
                if (Input.anyKey)
                {
                    asyncOperation.allowSceneActivation = true;
                    GameManager.Instance.StartGame();
                    gameObject.SetActive(false);
                }
            }
            yield return null;
        }
    }
}
