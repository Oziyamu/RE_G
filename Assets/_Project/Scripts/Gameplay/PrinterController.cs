using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PrinterController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 
        rb.gravityScale = 0f; 
        mainCamera = Camera.main; // Находим главную камеру
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // --- НОВЫЙ КОД: ПРИЦЕЛИВАНИЕ ---
        // Получаем позицию мышки в мире
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // Вычисляем направление от принтера к мышке
        Vector2 lookDir = mousePos - transform.position;
        // Вычисляем угол (в градусах)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        
        // Поворачиваем принтер плавно (для инерции можно добавить Mathf.Lerp позже)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }
}