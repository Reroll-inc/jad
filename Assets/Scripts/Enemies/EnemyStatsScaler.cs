using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(EnemyHpManager))]
public class EnemyStatsScaler : MonoBehaviour
{
    [Header("Speed scaling per level")]
    [SerializeField] private float speedBonusPerLevel = 0.08f;

    private AIPath aiPath;
    private EnemyHpManager hpManager;

    // EVERYTHING: This file could be part of the LevelManager file since its the level responsability
    // to know its number, difficulty and configure it all accordingly.
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        hpManager = GetComponent<EnemyHpManager>();

        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel == 0)
        {
            currentLevel = 1;
        }

        // Speed scaling logic
        float baseSpeed = aiPath.maxSpeed;
        float speedBonus = 1.0f + ((currentLevel - 1) * speedBonusPerLevel);
        aiPath.maxSpeed = baseSpeed * speedBonus;

        // Enemy HP scaling
        int baseHealth = hpManager.GetMaxHealth();
        int healthBonus = 0;

        if (currentLevel >= 14)
        {
            healthBonus = 2;
        }
        else if (currentLevel >= 7)
        {
            healthBonus = 1;
        }

        int finalHealth = baseHealth + healthBonus;
        hpManager.SetStartingHealth(finalHealth);
    }
}
