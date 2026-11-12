using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text energyText;
    public Text timerText;
    public Text healthText; // Новое поле
    public GameObject endScreen;       

    void OnEnable()
    {
        EventManager.OnEnergyChanged += UpdateEnergyUI;
        EventManager.OnCycleCompleted += ShowEndScreen;
        EventManager.OnHealthChanged += UpdateHealthUI;
    }

    void OnDisable()
    {
        EventManager.OnEnergyChanged -= UpdateEnergyUI;
        EventManager.OnCycleCompleted -= ShowEndScreen;
        EventManager.OnHealthChanged -= UpdateHealthUI;
    }

    void Start()
    {
        if (endScreen != null) endScreen.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameActive && timerText != null)
        {
            timerText.text = "TIME: " + Mathf.CeilToInt(GameManager.Instance.timeLeft);
        }
    }

    void UpdateEnergyUI(int amount)
    {
        if (energyText != null) energyText.text = "ENERGY: " + amount;
    }

    void UpdateHealthUI(int current, int max)
    {
        if (healthText != null) healthText.text = "HP: " + current + " / " + max;
    }

    void ShowEndScreen()
    {
        if (endScreen != null)
        {
            endScreen.SetActive(true);
            Text endText = endScreen.GetComponentInChildren<Text>();
            if (endText != null)
            {
                string reason = GameManager.Instance.currentHealth <= 0 ? "КОРАБЛЬ УНИЧТОЖЕН" : "ЦИКЛ ЗАВЕРШЕН";
                endText.text = reason + "\nСобрано энергии: " + GameManager.Instance.CurrentEnergy;
            }
        }
    }
}