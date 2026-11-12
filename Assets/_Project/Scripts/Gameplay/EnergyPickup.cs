using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    public int energyAmount = 10; // Сколько энергии дает

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если подобрал игрок (у него должен быть тег "Player")
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddEnergy(energyAmount);
            Destroy(gameObject); // Исчезает после подбора
        }
    }
}