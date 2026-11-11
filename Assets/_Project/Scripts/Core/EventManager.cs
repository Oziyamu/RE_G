using System;
using UnityEngine;

public static class EventManager
{
    // Событие: Энергия изменилась (передает текущее кол-во)
    public static Action<int> OnEnergyChanged;
    
    // Событие: Игровой цикл завершен (5 минут прошло)
    public static Action OnCycleCompleted;

    // Методы для вызова событий
    public static void EnergyChanged(int amount) => OnEnergyChanged?.Invoke(amount);
    public static void CycleCompleted() => OnCycleCompleted?.Invoke();
}