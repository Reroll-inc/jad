using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Wand : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;

    private Rigidbody2D pivot;
    private Rigidbody2D tipBody;
    private float lastShotTime = -Mathf.Infinity;
    private PlayerStats playerStats;

    private bool shooting = false;
    private Vector2 shootInput;

    void Start()
    {
        pivot = GetComponent<Rigidbody2D>();
        playerStats = GetComponentInParent<PlayerStats>();
        tipBody = transform.Find("Wand Tip").GetComponentInChildren<Rigidbody2D>();
    }

    void Update()
    {
        TryShooting();
    }

    void TryShooting()
    {
        if (!shooting || Time.time < lastShotTime + playerStats.ShootCooldown) return;

        // 1. We move the wand
        float angle = Mathf.Atan2(shootInput.y, shootInput.x) * Mathf.Rad2Deg;

        pivot.MoveRotation(-90 + angle);
        // 2. We shoot if CD is up
        lastShotTime = Time.time;
        shootInput = shootInput.normalized;

        if (Math.Abs(shootInput.y) >= Math.Abs(shootInput.x))
            shootInput.x = 0;
        else if (Math.Abs(shootInput.x) > Math.Abs(shootInput.y))
            shootInput.y = 0;

        GameObject bullet = Instantiate(bulletPrefab, tipBody.position, Quaternion.identity);

        bullet.transform.localScale = playerStats.BulletSize;

        if (bullet.TryGetComponent(out Rigidbody2D rbBullet))
        {
            rbBullet.linearVelocity = shootInput * playerStats.ShootSpeed;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            shooting = false;

            return;
        }
        if (context.performed)
        {
            shooting = true;
            shootInput = context.ReadValue<Vector2>();
        }
    }
}
