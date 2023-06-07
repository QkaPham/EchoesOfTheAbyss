using DG.Tweening;
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

    private void OnEnable()
    {
        Health.OnHealthChange += UpdateHPBar;
        Currency.OnCurrencyChange += UpdateFragmentText;
        RoundTimer.BossRoundStart += OnBossRoundStart;
    }

    private void OnDisable()
    {
        Health.OnHealthChange -= UpdateHPBar;
        Currency.OnCurrencyChange -= UpdateFragmentText;
        RoundTimer.BossRoundStart -= OnBossRoundStart;
    }

    private void UpdateHPBar(Health health)
    {
        float percentHealtPoint = Mathf.Clamp01(health.CurrentHealth / health.MaxHealth);
        hpBar.fillAmount = percentHealtPoint;
    }

    public void UpdateStaminaBar(Stamina stamina)
    {
        float percentStamina = Mathf.Clamp01(stamina.CurrentStamina / stamina.MaxStanima);
        staminaBar.fillAmount = percentStamina;
    }

    public void UpdateManaBar(Mana mana)
    {
        float percentMana = Mathf.Clamp01(mana.CurrentMana / mana.MaxMana);
        mpBar.fillAmount = percentMana;
    }

    public void UpdateRoundtimerText(float time)
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

    private void OnBossRoundStart()
    {
        ShowBossHPBar(true);
    }

    public void ShowBossHPBar(bool active)
    {
        bossHPBar.transform.parent.gameObject.SetActive(active);
    }

    private void UpdateBossHPBar(float remainHeath, float maxHeath)
    {
        float percentHealtPoint = Mathf.Clamp01(remainHeath / maxHeath);
        bossHPBar.fillAmount = percentHealtPoint;
    }
}
