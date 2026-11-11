using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Настройки оружия")]
    public GameObject bulletPrefab; // Сюда перетащим нашу пулю
    public Transform firePoint;     // Откуда стрелять
    public float fireRate = 0.2f;   // Задержка между выстрелами

    private float nextFireTime = 0f;

    void Update()
    {
        // Если нажали левую кнопку мыши и прошло достаточно времени
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
            // Создаем пулю
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}