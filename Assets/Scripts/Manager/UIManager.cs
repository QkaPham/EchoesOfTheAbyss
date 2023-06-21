using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Texture2D customCursor;
    [SerializeField] private Volume volume;
    private DepthOfField depthOfField;

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

        if (volume.profile.TryGet(out DepthOfField dof))
        {
            depthOfField = dof;
        }
    }

    private void Start()
    {
        depthOfField.active = false;
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
        depthOfField.active = active;
    }

    public void ClearHistoryViews()
    {
        history.Clear();
    }

    public void Show(View viewName, Action onComplete = null, bool remember = true)
    {
        StartCoroutine(DelayShow(viewName, onComplete, remember));
    }

    private IEnumerator DelayShow(View viewName, Action onComplete = null, bool remember = true)
    {
        yield return null;
        Debug.Log("Show View " + viewName.ToString());
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
        var view = history.Pop();
        Show(view.viewName, onComplete, false);
    }

    public bool CompareCurrentView(View view) => currentView.viewName == view;
}
