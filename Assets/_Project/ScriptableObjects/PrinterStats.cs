using UnityEngine;

[CreateAssetMenu(fileName = "PrinterStats", menuName = "Game/Printer Stats")]
public class PrinterStats : ScriptableObject
{
    public int cost = 250;
    public int maxHealth = 1000;
    public float extractionSpeed = 10f;
}