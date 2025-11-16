using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float shootSpeed = 10.0f;
    [SerializeField] private float shootCooldown = 0.8f;
    [SerializeField] private float dashCooldown = 2.5f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float dashVelocity = 35f;
    [SerializeField] private Vector2 bulletSize = new(1f, 1f);

    private float moveSpeedBonus = 0f;
    private float shootCooldownBonus = 0f;
    private float dashCooldownBonus = 0f;
    private float bulletSizeBonus = 0f;

    // Actual stats
    public float MoveSpeed { get; private set; }
    public float ShootSpeed { get; private set; }
    public float ShootCooldown { get; private set; }
    public float DashCooldown { get; private set; }
    public float DashDuration { get; private set; }
    public float DashVelocity { get; private set; }
    public Vector2 BulletSize { get; private set; }

    void Start()
    {
        RecalculateStats();
    }

    void RecalculateStats()
    {
        MoveSpeed = moveSpeed * (1f + moveSpeedBonus);
        ShootSpeed = shootSpeed;
        ShootCooldown = shootCooldown / (1f + shootCooldownBonus);
        DashCooldown = dashCooldown / (1f + dashCooldownBonus);
        DashDuration = dashDuration;
        DashVelocity = dashVelocity;
        BulletSize = bulletSize * (1f + bulletSizeBonus);
    }

    public void ApplyPowerUp(CardType selectedPowerUp)
    {
        switch (selectedPowerUp)
        {
            case CardType.Mage: // +10% ASPD
                shootCooldownBonus += 0.10f;
                Debug.Log("New ASPD Bonus: " + shootCooldownBonus);
                break;

            case CardType.Chariot: // +10% MSPD
                moveSpeedBonus += 0.10f;
                Debug.Log("New MSPD Bonus " + moveSpeedBonus);
                break;

            case CardType.Wheel: // -10% Dash CD
                dashCooldownBonus += 0.10f;
                Debug.Log("New DashCD Bonus: " + dashCooldownBonus);
                break;

            case CardType.Star: // +10% Bullet Size
                bulletSizeBonus += 0.05f;
                Debug.Log("New bullet size bonus: " + bulletSizeBonus);
                break;
        }

        RecalculateStats();
    }
}
