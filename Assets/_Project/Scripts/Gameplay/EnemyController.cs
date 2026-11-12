using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public GameObject energyPrefab; // Сюда кинем префаб энергии

    void Start()
    {
        currentHealth = maxHealth;
        // Чтобы пуля могла в него попасть, у врага должен быть тег "Enemy"!
        gameObject.tag = "Enemy"; 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Выбрасываем энергию
        if (energyPrefab != null)
        {
            Instantiate(energyPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // Уничтожаем врага
    }
}