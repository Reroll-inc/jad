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
    private SpriteRenderer sprite;
    private Color invisible;
    private float lastShotTime = -Mathf.Infinity;
    private PlayerStats playerStats;

    private bool shooting = false;
    private Vector2 shootInput;

    void Start()
    {
        pivot = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
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

        float angle = Mathf.Atan2(shootInput.y, shootInput.x) * Mathf.Rad2Deg;

        pivot.MoveRotation(-90 + angle);
        lastShotTime = Time.time;

        if(Math.Abs(pivot.rotation) == 180)
            sprite.sortingOrder = 1;
        else
            sprite.sortingOrder = -1;


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
            sprite.enabled = false;

            return;
        }
        if (context.performed)
        {
            shooting = true;
            sprite.enabled = true;
            shootInput = context.ReadValue<Vector2>();
        }
    }
}
