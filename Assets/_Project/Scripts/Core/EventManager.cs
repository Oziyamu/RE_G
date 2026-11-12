using System;
using UnityEngine;

public static class EventManager
{
    public static Action<int> OnEnergyChanged;
    public static Action OnCycleCompleted;
    public static Action<int, int> OnHealthChanged; // Текущее, Максимальное

    public static void EnergyChanged(int amount) => OnEnergyChanged?.Invoke(amount);
    public static void CycleCompleted() => OnCycleCompleted?.Invoke();
    public static void HealthChanged(int current, int max) => OnHealthChanged?.Invoke(current, max);
}