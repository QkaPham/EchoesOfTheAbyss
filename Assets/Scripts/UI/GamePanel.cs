using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    [SerializeField]
    private Image hpBar;

    [SerializeField]
    private Image mpBar;

    [SerializeField]
    private Image staminaBar;

    [SerializeField]
    private Image bossHPBar;

    [SerializeField]
    private TextMeshProUGUI fragmentText;

    [SerializeField]
    private TextMeshProUGUI roundtimerText;

    [SerializeField]
    private TextMeshProUGUI roundText;

    private Action<Notify> OnHealthChange, OnManaChange, OnStanimaChange, OnCurrencyChange;
    private Action<Notify> OnTimerChange, OnRoundChange;
    private Action<Notify> OnBossHealthChange;

    private void Awake()
    {
        OnHealthChange = thisNotify => { if (thisNotify is HealthChangeNotify notify) UpDateHPBar(notify.currentHealth / notify.maxHealth); };
        OnManaChange = thisNotify => { if (thisNotify is ManaChangeNotify notify) UpdateManaBar(notify.currentMana / notify.maxMana); };
        OnStanimaChange = thisNotify => { if (thisNotify is StaminaChangeNotify notify) UpdateStaminaBar(notify.currentStanima / notify.maxStamina); };
        OnCurrencyChange = thisNotify => { if (thisNotify is CurrencyChangeNotify notify) UpdateFragmentText(notify.balance); };

        OnTimerChange = thisNotify => { if (thisNotify is TimeChangeNotify notify) UpdateTimerText(notify.time); };
        OnRoundChange = thisNotify =>
        {
            if (thisNotify is RoundChangeNotify notify)
            {
                UpdateRoundText(notify.round);
                if (notify.isBossRound)
                {
                    ShowBossHPBar(true);
                    UpdateRoundtimerText("");
                }
                else
                {
                    ShowBossHPBar(false);
                }
            }
        };
        OnBossHealthChange = thisNotify => { if (thisNotify is HealthChangeNotify notify) UpdateBossHPBar(notify.currentHealth / notify.maxHealth); };
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(EventID.HealthChange, OnHealthChange);
        EventManager.Instance.AddListener(EventID.ManaChange, OnManaChange);
        EventManager.Instance.AddListener(EventID.StaminaChange, OnStanimaChange);
        EventManager.Instance.AddListener(EventID.CurrencyChange, OnCurrencyChange);

        EventManager.Instance.AddListener(EventID.TimerChange, OnTimerChange);
        EventManager.Instance.AddListener(EventID.RoundChange, OnRoundChange);

        EventManager.Instance.AddListener(EventID.BossHealthChange, OnBossHealthChange);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(EventID.HealthChange, OnHealthChange);
        EventManager.Instance.RemoveListener(EventID.ManaChange, OnManaChange);
        EventManager.Instance.RemoveListener(EventID.StaminaChange, OnStanimaChange);
        EventManager.Instance.RemoveListener(EventID.CurrencyChange, OnCurrencyChange);

        EventManager.Instance.RemoveListener(EventID.TimerChange, OnTimerChange);
        EventManager.Instance.RemoveListener(EventID.RoundChange, OnRoundChange);

        EventManager.Instance.RemoveListener(EventID.BossHealthChange, OnBossHealthChange);
    }

    private void Update()
    {
        if (InputManager.Instance.Cancel)
        {
            if (UIManager.Instance.CompareCurrentView(View.Game))
            {
                OnPauseButtonClick();
            }
        }
    }

    private void OnPauseButtonClick()
    {
        GameManager.Instance.Pause();
    }

    public void UpDateHPBar(float value)
    {
        hpBar.fillAmount = value;
    }

    public void UpdateManaBar(float value)
    {
        mpBar.fillAmount = value;
    }

    public void UpdateStaminaBar(float value)
    {
        staminaBar.fillAmount = value;
    }

    public void UpdateTimerText(float time)
    {
        time = Mathf.Clamp(time, 0f, float.MaxValue);
        UpdateRoundtimerText(Mathf.Ceil(time).ToString());
    }

    public void UpdateRoundtimerText(string time)
    {
        roundtimerText.text = time;
    }

    public void UpdateRoundText(int round)
    {
        roundText.text = $"Round {round}";
    }

    private void UpdateFragmentText(int currency)
    {
        fragmentText.text = currency.ToString();
    }

    public void ShowBossHPBar(bool active)
    {
        bossHPBar.transform.parent.gameObject.SetActive(active);
    }

    private void UpdateBossHPBar(float value)
    {
        bossHPBar.fillAmount = value;
    }
}
