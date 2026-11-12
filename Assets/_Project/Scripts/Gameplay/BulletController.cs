using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 20; // Урон пули

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если попали во врага — наносим урон
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null) enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}