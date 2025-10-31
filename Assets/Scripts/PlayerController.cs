using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(HealthManager))]
public class PlayerController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float shotSpeed = 10.0f;
    [SerializeField] private float shotCooldown = 0.5f;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Animation States")]
    [SerializeField] private string running = "Running";
    [SerializeField] private string facing = "CurrentInput";
    [SerializeField] private string lastFaced = "LastInput";

    private HealthManager healthManager;
    private Rigidbody2D body;
    private Rigidbody2D wand;
    private Rigidbody2D wandTip;
    private SpriteRenderer wandSprite;
    private Animator animator;
    private Vector2 playerInput;
    private Vector2 shootInput;
    private bool shooting = false;
    private float lastShotTime = -Mathf.Infinity;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        wand = transform.Find("Wand Pivot").GetComponentInChildren<Rigidbody2D>();
        wandTip = transform.Find("Wand Pivot/Wand Tip").GetComponentInChildren<Rigidbody2D>();
        wandSprite = transform.Find("Wand Pivot/Wand Sprite").GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        healthManager = GetComponent<HealthManager>();
    }

    void Update()
    {
        if (Time.time >= lastShotTime + shotCooldown && shooting)
        {
            GameObject bullet = Instantiate(bulletPrefab, wandTip.position, Quaternion.identity);

            if (bullet.TryGetComponent<Rigidbody2D>(out var rbBullet))
            {
                rbBullet.linearVelocity = shootInput * shotSpeed;
            }
            lastShotTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        body.linearVelocity = moveSpeed * playerInput;
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

    /*public void ApplyPowerUp(CardType selectedPowerUp)
    {
        switch (selectedPowerUp)
        {
            case CardType.Mage:
                activeBullets++;
                break;
            
            case CardType.Chariot:
                moveSpeed++;
                break;
            
            case CardType.Wheel:
                dashCD--;
                break;
            
            case CardType.Star:
                bulletSize++;
                break; 
        }

    }*/
}
