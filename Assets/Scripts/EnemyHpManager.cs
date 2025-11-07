using UnityEngine;
using UnityEngine.Events;

public class EnemyHpManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;

    private int currentHealth;

    public UnityEvent onDeath;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ReceiveDamage(int damage)
    {
        // Discount damage received
        currentHealth -= damage;

        // Avoid HP below 0 on specific scenarios (2 ticks on same frame)
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log(gameObject.name + "received" + damage + " de daño. Vida restante: " + currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + "murió");

        if (onDeath != null)
        {
            onDeath.Invoke();
        }
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetStartingHealth(int newMaxHealth) // Used for HP scaling
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
    }
}
