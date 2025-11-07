using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float shotSpeed = 10.0f;
    [SerializeField] private float shotCooldown = 0.8f;
    [SerializeField] private float dashCooldown = 2.5f;
        
    [HideInInspector] public GameObject bulletPrefab;

    // Saved base Stats for future calculations
    private float baseMoveSpeed;
    private float baseShotCooldown;
    private float baseDashCooldown;
    private Vector2 baseBulletSize;

    // Stacked bonus stats
    private float moveSpeedBonus = 0f;
    private float shotCooldownBonus = 0f;
    private float dashCooldownBonus = 0f;
    private float bulletSizeBonus = 0f;

    // Actual stats
    public float CurrentMoveSpeed { get; private set; }
    public float ShotSpeed { get; private set; }
    public float CurrentShotCooldown { get; private set; }
    public float CurrentDashCooldown { get; private set; }
    public Vector2 CurrentBulletSize {  get; private set; }

    void Awake()
    {
        baseMoveSpeed = moveSpeed;
        baseShotCooldown = shotCooldown;
        baseDashCooldown = dashCooldown;
        ShotSpeed = shotSpeed;

        RecalculateStats();
    }
        
    public void Initialize(GameObject bulletPrefabRef)
    {
        bulletPrefab = bulletPrefabRef;
        if (bulletPrefab != null )
        {
            baseBulletSize = bulletPrefab.transform.localScale;
            CurrentBulletSize = baseBulletSize;
        }
    }

    private void RecalculateStats()
    {
        CurrentMoveSpeed = baseMoveSpeed * (1f + moveSpeedBonus);
        CurrentShotCooldown = baseShotCooldown / (1f + shotCooldownBonus);
        CurrentDashCooldown = baseDashCooldown / (1f + dashCooldownBonus);
        CurrentBulletSize = baseBulletSize * (1f + bulletSizeBonus);
    }

    public void ApplyPowerUp(CardType selectedPowerUp)
    {
        switch (selectedPowerUp)
        {
            case CardType.Mage: // +10% ASPD
                shotCooldownBonus += 0.10f;
                Debug.Log("New ASPD Bonus: " + shotCooldownBonus);
                break;

            case CardType.Chariot: // +10% MSPD
                moveSpeedBonus += 0.10f;
                Debug.Log("New MSPD Bonus " +  moveSpeedBonus);
                break;

            case CardType.Wheel: // -10% Dash CD
                dashCooldownBonus += 0.10f;
                Debug.Log("New DashCD Bonus: " + dashCooldownBonus);
                break;

            case CardType.Star: // +10% Bullet Size
                bulletSizeBonus += 0.05f; // sqrt(3). Fits the +10% bonus
                Debug.Log("New bullet size bonus: " + bulletSizeBonus);
                break;                        
        }
        RecalculateStats();
    }

    void Start()
    {
        
    }
        
    void Update()
    {
        
    }
}
