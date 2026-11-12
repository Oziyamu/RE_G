using UnityEngine;

public class CockpitUIManager : MonoBehaviour
{
    public RectTransform leverTransform; // Наш рычаг
    public float pullDistance = 20f;     // Насколько сильно дергается рычаг
    public float returnSpeed = 10f;      // Как быстро он возвращается

    private Vector2 originalPosition;
    private bool isPulled = false;

    void Start()
    {
        if (leverTransform != null)
            originalPosition = leverTransform.anchoredPosition;
    }

    void Update()
    {
        if (leverTransform == null) return;

        // Если идет стрельба (нажата ЛКМ)
        if (Input.GetMouseButton(0))
        {
            isPulled = true;
        }

        if (isPulled)
        {
            // Опускаем рычаг вниз
            leverTransform.anchoredPosition = Vector2.Lerp(leverTransform.anchoredPosition, originalPosition + new Vector2(0, -pullDistance), Time.deltaTime * 20f);
            
            // Если отпустили кнопку — возвращаем обратно
            if (!Input.GetMouseButton(0))
            {
                isPulled = false;
            }
        }
        else
        {
            // Возвращаем рычаг на место
            leverTransform.anchoredPosition = Vector2.Lerp(leverTransform.anchoredPosition, originalPosition, Time.deltaTime * returnSpeed);
        }
    }
}