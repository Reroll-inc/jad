using System;
using UnityEngine;
using UnityEngine.Events;

public enum HealthOperation { Inc, Dec }

public class PlayerHpManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth = 3;
    public UnityEvent OnHealthChange;
    public UnityEvent OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void IncrementMaxHealth()
    {
        if (maxHealth == 7) return;

        maxHealth++;
        currentHealth++;

        OnHealthChange.Invoke();
    }

    public void UpdateCurrentHealth(HealthOperation type)
    {
        int previousHealth = currentHealth;

        switch (type)
        {
            case HealthOperation.Inc:
                currentHealth = Math.Min(currentHealth + 1, maxHealth);
                break;
            case HealthOperation.Dec:
                currentHealth = Math.Max(currentHealth - 1, 0);
                break;
        }

        if (previousHealth != currentHealth)
            OnHealthChange.Invoke();

        if (currentHealth == 0)
            OnDeath.Invoke();
    }
}
