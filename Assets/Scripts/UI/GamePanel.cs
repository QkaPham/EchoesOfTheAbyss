using TMPro;
using UnityEngine;
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
    private TextMeshProUGUI fragmentText;

    [SerializeField]
    private TextMeshProUGUI roundtimerText;

    [SerializeField]
    private TextMeshProUGUI roundText;

    private void OnEnable()
    {
        Health.OnHealthChange += UpdateHPBar;
        RoundTimer.OnTimerUpdate += UpdateRoundtimerText;
        Currency.OnCurrencyChange += UpdateFragmentText;
    }

    private void OnDisable()
    {
        Health.OnHealthChange -= UpdateHPBar;
        RoundTimer.OnTimerUpdate -= UpdateRoundtimerText;
        Currency.OnCurrencyChange -= UpdateFragmentText;
    }

    private void UpdateHPBar(float damage, float remainHeath, float maxHeath)
    {
        float percentHealtPoint = Mathf.Clamp01(remainHeath / maxHeath);
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

    private void UpdateRoundtimerText(float time)
    {
        time = Mathf.Clamp(time, 0f, float.MaxValue);
        roundtimerText.text = Mathf.Ceil(time).ToString();
    }

    public void UpdateRoundText(int round)
    {
        roundText.text = $"Round {round}";
    }

    private void UpdateFragmentText(int currency)
    {
        fragmentText.text = currency.ToString();
    }
}
