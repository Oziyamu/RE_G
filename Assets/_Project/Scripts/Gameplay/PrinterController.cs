using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PrinterController : MonoBehaviour
{
    [Header("Настройки движения (Шагоход)")]
    public float maxSpeed = 6f;          
    public float acceleration = 15f;     
    public float deceleration = 8f;      
    
    [Header("Настройки поворота")]
    public float turnSpeed = 10f; // Чем меньше, тем тяжелее и плавнее поворот

    private Rigidbody2D rb;
    private Vector2 currentVelocity;     
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; 
        rb.gravityScale = 0f; 
        mainCamera = Camera.main; 
    }

    void Update()
    {
        // Прицеливание оставляем в Update (для отзывчивости мышки)
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float targetAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        // ПЛАВНЫЙ поворот (вместо мгновенного)
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // Читаем ввод внутри FixedUpdate (синхронизация с физикой)
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 targetVelocity = input.normalized * maxSpeed;

        if (input.magnitude > 0.1f)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        rb.linearVelocity = currentVelocity;
    }
}