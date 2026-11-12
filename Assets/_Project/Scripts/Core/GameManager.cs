using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentEnergy { get; private set; } = 0;
    public float timeLeft = 30f; 
    public bool isGameActive = true;

    [Header("Здоровье игрока")]
    public int maxHealth = 100;
    public int currentHealth;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        EventManager.HealthChanged(currentHealth, maxHealth); // Обновляем UI при старте
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

    public void TakeDamage(int amount)
    {
        if (!isGameActive) return;

        currentHealth -= amount;
        EventManager.HealthChanged(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isGameActive = false;
            Debug.Log("Игрок уничтожен! Игра окончена.");
            EventManager.CycleCompleted(); // Показываем экран итогов
        }
    }

    private void EndCycle()
    {
        isGameActive = false;
        Debug.Log("Цикл завершен! Энергия: " + CurrentEnergy);
        EventManager.CycleCompleted(); 
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}