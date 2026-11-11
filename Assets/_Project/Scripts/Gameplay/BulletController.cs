using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 10f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Пуля не падает вниз
        // Летим туда, куда смотрит пуля (её ось Y)
        rb.linearVelocity = transform.up * speed;
        
        // Уничтожаем пулю через 2 секунды, чтобы не засорять память
        Destroy(gameObject, 2f);
    }

    // Позже здесь будет логика урона
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если во что-то попали — уничтожаем пулю
        Destroy(gameObject);
    }
}