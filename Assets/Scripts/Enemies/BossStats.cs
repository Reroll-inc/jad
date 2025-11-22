using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float fireballSpeed = 10.0f;
    [SerializeField] private float fireballCD = 3f;
    [SerializeField] private int teletransportCD = 5;
    [SerializeField] private float velocityPenalization = 0.015f;
    [SerializeField] private float cdPenalization = 0.005f;

    // Actual stats
    public float FireballSpeed { get; private set; }
    public float FireballCD { get; private set; }
    public float TeletransportCD { get; private set; }

    void Awake()
    {
        FireballCD = fireballCD;
    }

    void Start()
    {
        FireballSpeed = fireballSpeed;
        TeletransportCD = teletransportCD;
    }

    public void TeleportPenalization()
    {
        FireballSpeed += fireballSpeed * velocityPenalization;
        FireballCD -= fireballCD * cdPenalization;
    }

    // TODO: Add spell sequence
}
