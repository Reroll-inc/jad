using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerHpManager))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Animation States")]
    [SerializeField] private string running = "Running";
    [SerializeField] private string facing = "CurrentInput";
    [SerializeField] private string lastFaced = "LastInput";

    private PlayerHpManager healthManager;
    private Rigidbody2D body;
    private Rigidbody2D wand;
    private Rigidbody2D wandTip;
    private SpriteRenderer wandSprite;
    private Animator animator;
    private Vector2 playerInput;
    private Vector2 shootInput;
    private bool shooting = false;
    private float lastShotTime = -Mathf.Infinity;

    private PlayerStats playerStats;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        wand = transform.Find("Wand Pivot").GetComponentInChildren<Rigidbody2D>();
        wandTip = transform.Find("Wand Pivot/Wand Tip").GetComponentInChildren<Rigidbody2D>();
        wandSprite = transform.Find("Wand Pivot/Wand Sprite").GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        healthManager = GetComponent<PlayerHpManager>();
        playerStats = GetComponent<PlayerStats>();
        playerStats.Initialize(bulletPrefab);
    }

    void Update()
    {
        if (Time.time >= lastShotTime + playerStats.CurrentShotCooldown && shooting)
        {
            GameObject bullet = Instantiate(bulletPrefab, wandTip.position, Quaternion.identity);

            bullet.transform.localScale = playerStats.CurrentBulletSize;

            if (bullet.TryGetComponent(out Rigidbody2D rbBullet))
            {
                rbBullet.linearVelocity = shootInput * playerStats.ShotSpeed;
            }
            lastShotTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        body.linearVelocity = playerStats.CurrentMoveSpeed * playerInput;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        animator.SetBool(running, true);

        if (context.canceled)
        {
            animator.SetBool(running, false);
            if (playerInput.x != 0)
            {
                animator.SetFloat(lastFaced, playerInput.x);
            }
        }

        playerInput = context.ReadValue<Vector2>();
        animator.SetFloat(facing, playerInput.x);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        shooting = true;
        shootInput = context.ReadValue<Vector2>();
        if (shootInput.y < 0)
            wandSprite.sortingOrder = 1;
        else
            wandSprite.sortingOrder = -1;

        if (Math.Abs(shootInput.y) >= Math.Abs(shootInput.x))
            shootInput.x = 0;
        else if (Math.Abs(shootInput.x) > Math.Abs(shootInput.y))
            shootInput.y = 0;

        if (!context.canceled)
        {
            float angle = Mathf.Atan2(shootInput.y, shootInput.x) * Mathf.Rad2Deg;
            wand.MoveRotation(-90 + angle);
        }
        else if (context.canceled)
        {
            shooting = false;
        }
    }
}
