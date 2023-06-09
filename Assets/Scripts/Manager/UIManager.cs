using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using static Unity.Collections.AllocatorManager;
using Unity.VisualScripting;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Texture2D customCursor;
    [SerializeField] private Volume volume;
    private DepthOfField depthOfField;
    private Bloom bloom;

    [SerializeField] private View startView;
    private BaseView currentView;
    private Stack<BaseView> history = new Stack<BaseView>();
    private Dictionary<View, BaseView> viewsMap = new Dictionary<View, BaseView>();


    protected override void Awake()
    {
        base.Awake();

        if (customCursor != null)
        {
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
        if (canvas == null)
        {
            canvas = GetComponentInChildren<Canvas>().transform as RectTransform;
        }
        ActiveAllPanel();

        List<BaseView> baseViews = GetComponentsInChildren<BaseView>().ToList();
        foreach (BaseView view in baseViews)
        {
            if (view.viewName != View.None)
            {
                viewsMap.Add(view.viewName, view);
            }
        }
        currentView = viewsMap.GetValueOrDefault(startView);

        volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out depthOfField);
        volume.profile.TryGet(out bloom);
    }

    private void ActiveAllPanel()
    {
        foreach (Transform child in canvas)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void ActiveDepthOfField(bool active)
    {
        if (depthOfField == null) return;
        StartCoroutine(DelayActiveDepthOfField(active));
    }
    public void ActiveBloom(bool active)
    {
        if (bloom == null) return;
        bloom.active = active;
    }

    private IEnumerator DelayActiveDepthOfField(bool active)
    {
        if (active)
        {
            while (depthOfField.focalLength.value < 50)
            {
                depthOfField.focalLength.value += Time.deltaTime * 50;
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
        }
        else
        {
            while (depthOfField.focalLength.value > 1)
            {
                depthOfField.focalLength.value -= Time.deltaTime * 50;
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
        }
    }

    public void ClearHistoryViews()
    {
        history.Clear();
    }

    public void Show(View viewName, Action onComplete = null, bool remember = true, float delayTime = 0f)
    {
        StartCoroutine(DelayShow(viewName, onComplete, remember, delayTime));
    }

    private IEnumerator DelayShow(View viewName, Action onComplete = null, bool remember = true, float delayTime = 0f)
    {
        yield return null;
        yield return new WaitForSeconds(delayTime);
        viewsMap.TryGetValue(viewName, out BaseView view);
        if (view != null)
        {
            currentView.DeActivate(() =>
            {
                view.Activate(onComplete);
            });
            if (remember) history.Push(currentView);
            currentView = view;
        }
        else
        {
            Debug.LogError($"Not contain {viewName} view in views map");
        }
    }

    public void ShowLast(Action onComplete = null)
    {
        if (history.Peek() != null)
        {
            Show(history.Pop().viewName, onComplete, false);
        }
    }

    public bool CompareCurrentView(View view) => currentView.viewName == view;
}
