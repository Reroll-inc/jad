using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float movePenalization = 0.5f;

    [Header("Attack Stats")]
    [SerializeField] private float shootSpeed = 10.0f;
    [SerializeField] private float shootCooldown = 0.8f;
    [SerializeField] private Vector2 bulletSize = new(1f, 1f);
    [Tooltip("In seconds")]
    [SerializeField] private int attackPenalization = 1;

    [Header("Dash Stats")]
    [SerializeField] private float dashCooldown = 2.5f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashVelocity = 20f;

    private float moveSpeedBonus = 0f;
    private float shootCooldownBonus = 0f;
    private float dashCooldownBonus = 0f;
    private float bulletSizeBonus = 0f;

    // Public stats
    public float MoveSpeed { get; private set; }
    public float MovePenalization { get; private set; }

    public float ShootSpeed { get; private set; }
    public float ShootCooldown { get; private set; }
    public Vector2 BulletSize { get; private set; }
    public int AttackPenalization { get; private set; }

    public float DashCooldown { get; private set; }
    public float DashDuration { get; private set; }
    public float DashVelocity { get; private set; }

    void Start()
    {
        RecalculateStats();

        MovePenalization = movePenalization;
        ShootSpeed = shootSpeed;
        DashDuration = dashDuration;
        DashVelocity = dashVelocity;
        AttackPenalization = attackPenalization;
    }

    void RecalculateStats()
    {
        MoveSpeed = moveSpeed * (1f + moveSpeedBonus);
        ShootCooldown = shootCooldown / (1f + shootCooldownBonus);
        DashCooldown = dashCooldown / (1f + dashCooldownBonus);
        BulletSize = bulletSize * (1f + bulletSizeBonus);
    }

    public void ApplyPowerUp(CardType selectedPowerUp)
    {
        switch (selectedPowerUp)
        {
            case CardType.Mage:
                shootCooldownBonus += 0.10f;
                Debug.Log($"New ASPD Bonus: {shootCooldownBonus}");
                break;
            case CardType.Chariot:
                moveSpeedBonus += 0.10f;
                Debug.Log($"New MSPD Bonus {moveSpeedBonus}");
                break;
            case CardType.Wheel:
                dashCooldownBonus += 0.10f;
                Debug.Log($"New DashCD Bonus: {dashCooldownBonus}");
                break;
            case CardType.Star:
                bulletSizeBonus += 0.05f;
                Debug.Log($"New bullet size bonus: {bulletSizeBonus}");
                break;
        }
        RecalculateStats();

        Debug.Log($"ASPD: {ShootCooldown}");
        Debug.Log($"MSPD: {MoveSpeed}");
        Debug.Log($"DashCD: {DashCooldown}");
        Debug.Log($"BulletSize: {BulletSize}");
    }
}
