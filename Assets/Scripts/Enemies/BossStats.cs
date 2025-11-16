using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float fireballSpeed = 10.0f;
    [SerializeField] private float fireballCD = 3f;

    // Actual stats
    public float FireballSpeed { get; private set; }
    public float FireballCD { get; private set; }

    void Start()
    {
        FireballSpeed = fireballSpeed;
        FireballCD = fireballCD;
    }
    // TODO: Add spell sequence
}
