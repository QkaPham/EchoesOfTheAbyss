using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    [SerializeField]
    private Image HPBar;

    [SerializeField]
    private Image MPBar;

    [SerializeField]
    private Image StaminaBar;

    [SerializeField]
    private TextMeshProUGUI FragmentText;

    [SerializeField]
    private TextMeshProUGUI RoundtimerText;

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
        HPBar.fillAmount = percentHealtPoint;
    }

    public void UpdateStaminaBar(Stamina stamina)
    {
        float percentStamina = Mathf.Clamp01(stamina.CurrentStamina / stamina.MaxStanima);
        StaminaBar.fillAmount = percentStamina;
    }

    public void UpdateManaBar(Mana mana)
    {
        float percentMana = Mathf.Clamp01(mana.CurrentMana / mana.MaxMana);
        MPBar.fillAmount = percentMana;
    }

    private void UpdateRoundtimerText(float time)
    {
        time = Mathf.Clamp(time, 0f, float.MaxValue);
        RoundtimerText.text = Mathf.Ceil(time).ToString();
    }

    private void UpdateFragmentText(int currency)
    {
        FragmentText.text = currency.ToString();
    }
}
