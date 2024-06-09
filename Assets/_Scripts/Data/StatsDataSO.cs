using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Data/Player Stats")]
public class StatsDataSO : ScriptableObject
{
    public int CurrentHealth { get; private set; }
    public int CurrentCoins { get; private set; }

    public bool PlayerDead { get; private set; }

    [SerializeField] private int _maxHealth = 3;

    private void OnEnable()
    {
        PlayerDead = false;
        CurrentHealth = _maxHealth;
        CurrentCoins = 0;
    }

    public void OnHealthIncrease(int amount)
    {
        if (CurrentHealth >= _maxHealth) return;

        CurrentHealth += amount;
    }

    public void OnHealthDecrease(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
            PlayerDead = true;
    }

    public void OnCoinIncrease(int amount)
    {
        CurrentCoins += amount;
    }
}
