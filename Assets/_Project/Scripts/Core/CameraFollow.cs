using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Настройки тряски")]
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;

    void LateUpdate() 
    {
        if (target == null) return;

        // Базовая позиция (следуем за игроком)
        Vector3 targetPos = target.position + offset;

        // Добавляем тряску, если она активна
        if (shakeDuration > 0)
        {
            // Случайное смещение
            Vector3 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
            targetPos += shakeOffset;
            
            shakeDuration -= Time.deltaTime;
        }

        transform.position = targetPos;
    }

    // Метод, который вызовет оружие при выстреле
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}