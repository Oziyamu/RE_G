using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Настройки оружия")]
    public GameObject bulletPrefab; 
    public Transform firePoint;     
    public float fireRate = 0.2f;   

    [Header("Отдача (Juice)")]
    public float shakeDuration = 0.05f; // Было 0.1f
    public float shakeMagnitude = 0.05f; // Было 0.15f

    private float nextFireTime = 0f;
    private CameraFollow camFollow;

    void Start()
    {
        // Находим камеру один раз при старте
        camFollow = Camera.main.GetComponent<CameraFollow>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            // ТРЯСКА КАМЕРЫ
            if (camFollow != null)
            {
                camFollow.TriggerShake(shakeDuration, shakeMagnitude);
            }
        }
    }
}