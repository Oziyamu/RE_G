using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentEnergy { get; private set; } = 0;

    private void Awake()
    {
        // Делаем так, чтобы GameManager был один на всю игру
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddEnergy(int amount)
    {
        CurrentEnergy += amount;
        EventManager.EnergyChanged(CurrentEnergy); // Кричим всем: "Энергия изменилась!"
        Debug.Log($"Энергия: {CurrentEnergy}");
    }
}