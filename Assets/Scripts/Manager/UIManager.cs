using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private RectTransform CanvasTransform;

    [SerializeField]
    private Texture2D customCursor;

    [field: SerializeField]
    public MainMenuPanel MainMenuPanel { get; set; }

    [field: SerializeField]
    public GamePanel GamePanel { get; set; }

    [field: SerializeField]
    public PausePanel PausePanel { get; set; }

    [field: SerializeField]
    public SettingsPanel SettingsPanel { get; set; }

    [field: SerializeField]
    public UpgradePanel UpgradePanel { get; set; }

    [field: SerializeField]
    public GameOverPanel GameoverPanel { get; set; }

    [field: SerializeField]
    public VictoryPanel VictoryPanel { get; set; }

    [field: SerializeField]
    public LoadingPanel LoadingPanel { get; set; }

    [field: SerializeField]
    public FadePanel FadePanel { get; set; }

    [SerializeField]
    private Volume volume;
    [SerializeField]
    private DepthOfField depthOfField;


    public MainMenuView mainMenuView;
    public SettingsView settingsView;

    public BaseView startingView;
    public BaseView currentView;
    public Stack<BaseView> history = new Stack<BaseView>();
    private Dictionary<View, BaseView> viewsMap = new Dictionary<View, BaseView>();

    protected override void Awake()
    {
        base.Awake();
        if (customCursor != null)
        {
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
        if (CanvasTransform == null)
        {
            CanvasTransform = GetComponentInChildren<Canvas>().transform as RectTransform;
        }

        ActiveAllPanel();

        MainMenuPanel = GetComponentInChildren<MainMenuPanel>();
        GamePanel = GetComponentInChildren<GamePanel>();
        PausePanel = GetComponentInChildren<PausePanel>();
        SettingsPanel = GetComponentInChildren<SettingsPanel>();
        UpgradePanel = GetComponentInChildren<UpgradePanel>();
        GameoverPanel = GetComponentInChildren<GameOverPanel>();
        VictoryPanel = GetComponentInChildren<VictoryPanel>();
        LoadingPanel = GetComponentInChildren<LoadingPanel>();
        FadePanel = GetComponentInChildren<FadePanel>();

        mainMenuView = GetComponentInChildren<MainMenuView>();
        settingsView = GetComponentInChildren<SettingsView>();

        List<BaseView> baseViews = GetComponentsInChildren<BaseView>().ToList();
        foreach (BaseView view in baseViews)
        {
            if (view.viewName != View.None)
            {
                viewsMap.Add(view.viewName, view);
            }
        }
        currentView = startingView;
        history.Push(startingView);

        MainMenuPanel.Activate(true, 1f);
        GamePanel.Activate(false);
        PausePanel.Activate(false);
        SettingsPanel.Activate(false);
        UpgradePanel.Activate(false);
        GameoverPanel.Activate(false);
        VictoryPanel.Activate(false);

        volume = GetComponentInChildren<Volume>();
        if (volume.profile.TryGet<DepthOfField>(out DepthOfField dof))
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
        foreach (Transform child in CanvasTransform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void ActiveDepthOfField(bool active)
    {
        depthOfField.active = active;
    }

    public void LoadScene(string scene, View viewName)
    {
        //Show(View.Load, null, false);
        LoadingPanel.LoadScene(scene, viewName);
    }

    public void Fade(float value, float fadeTime)
    {
        FadePanel.Fade(value, fadeTime);
    }

    public void Show(View viewName, Action onComplete = null, bool remember = true)
    {
        viewsMap.TryGetValue(viewName, out BaseView view);
        if (view != null)
        {
            currentView.DeActivate(() =>
            {
                view.Activate(onComplete);
            });
            if (remember)
            {
                history.Push(currentView);
            }
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


}
