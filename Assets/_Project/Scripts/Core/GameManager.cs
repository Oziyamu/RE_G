using UnityEngine;
using UnityEngine.SceneManagement; // Для перезагрузки сцены

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentEnergy { get; private set; } = 0;
    public float timeLeft = 30f; // Для теста 30 сек, в релизе сделаем 300 (5 мин)
    public bool isGameActive = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!isGameActive) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            EndCycle();
        }
    }

    public void AddEnergy(int amount)
    {
        CurrentEnergy += amount;
        EventManager.EnergyChanged(CurrentEnergy);
    }

    private void EndCycle()
    {
        isGameActive = false;
        Debug.Log("Цикл завершен! Энергия: " + CurrentEnergy);
        EventManager.CycleCompleted(); // Отправляем событие для UI
    }
    
    // Метод для кнопки "Заново" в UI
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}