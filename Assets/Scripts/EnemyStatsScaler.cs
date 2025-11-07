using UnityEngine;
using UnityEngine.SceneManagement; //Reads Scene index
using Pathfinding; // AIPath reference

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(EnemyHpManager))]
public class EnemyStatsScaler : MonoBehaviour
{
    [Header("Speed scaling per level")]
    [SerializeField] private float speedBonusPerLevel = 0.025f; // +2.5% Enemy MSPD per level

    private AIPath aiPath;
    private EnemyHpManager hpManager;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        hpManager = GetComponent<EnemyHpManager>();
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (currentLevel == 0)
        {
            currentLevel = 1; //Avoids menu screen (0)
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
