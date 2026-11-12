using UnityEngine;
using UnityEngine.UI; // Используем стандартный UI

public class UIManager : MonoBehaviour
{
    public Text energyText; // Изменено с TextMeshProUGUI
    public Text timerText;  // Изменено с TextMeshProUGUI
    public GameObject endScreen;       

    void OnEnable()
    {
        EventManager.OnEnergyChanged += UpdateEnergyUI;
        EventManager.OnCycleCompleted += ShowEndScreen;
    }

    void OnDisable()
    {
        EventManager.OnEnergyChanged -= UpdateEnergyUI;
        EventManager.OnCycleCompleted -= ShowEndScreen;
    }

    void Start()
    {
        if (endScreen != null) endScreen.SetActive(false);
        UpdateEnergyUI(GameManager.Instance.CurrentEnergy);
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
        if (energyText != null)
            energyText.text = "ENERGY: " + amount;
    }

    void ShowEndScreen()
    {
        if (endScreen != null)
        {
            endScreen.SetActive(true);
            // Ищем текст внутри экрана итогов и меняем его
            Text endText = endScreen.GetComponentInChildren<Text>();
            if (endText != null)
                endText.text = "Цикл завершен!\nСобрано энергии: " + GameManager.Instance.CurrentEnergy;
        }
    }
}