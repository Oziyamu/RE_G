using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Требуем наличие физики
public class EnemyController : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public GameObject energyPrefab;
    
    [Header("ИИ")]
    public float moveSpeed = 2f;
    public int damageToPlayer = 10;
    public float attackCooldown = 1f;

    private Transform playerTransform;
    private float nextAttackTime = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true; // Чтобы враги не крутились при столкновениях

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    // ВАЖНО: Движение физики делаем в FixedUpdate
    void FixedUpdate() 
    {
        if (playerTransform != null && GameManager.Instance.isGameActive)
        {
            // Вычисляем направление к игроку и задаем скорость
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (energyPrefab != null) Instantiate(energyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Этот метод теперь будет срабатывать, потому что у врага есть Rigidbody2D
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            GameManager.Instance.TakeDamage(damageToPlayer);
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log("Враг атакует!"); // Будем видеть в консоли
        }
    }
}