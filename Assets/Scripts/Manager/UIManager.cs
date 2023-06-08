using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

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

    public void LoadScene(string scene)
    {
        LoadingPanel.LoadScene(scene);
    }

    public void Fade(float value, float fadeTime, Action onComplete = null)
    {
        FadePanel.Fade(value, fadeTime, onComplete);
    }
}
