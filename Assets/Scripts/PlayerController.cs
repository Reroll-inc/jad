using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
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
    private Vector2 lastMoveInput;
    private float lastShotTime = -Mathf.Infinity;
    private PlayerStats playerStats;
    private InputAction moveAction;
    private InputAction shootAction;
    private InputAction pauseAction;

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
        PlayerInput playerInput = GameManager.Instance.playerInput; // GameManager controls player inputs
        InputActionMap gameplayMap = playerInput.actions.FindActionMap("Gameplay");
        moveAction = gameplayMap.FindAction("Move");
        shootAction = gameplayMap.FindAction("Shoot");
        pauseAction = gameplayMap.FindAction("Pause");
    }

    void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            GameManager.Instance.TogglePause();
            return;
        }

        // Input reading
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 shootInput = shootAction.ReadValue<Vector2>();
        bool isShooting = shootAction.IsPressed();

        OnMoveAnimation(moveInput);
        WandRotation(shootInput);
        OnShooting(shootInput, isShooting);
    }

    void FixedUpdate()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        body.linearVelocity = playerStats.CurrentMoveSpeed * moveInput;
    }

    private void OnMoveAnimation(Vector2 moveInput)
    {
        animator.SetFloat(facing, moveInput.x);
        if (moveInput.magnitude > 0.1f) // if is moving
        {
            animator.SetBool(running, true);
            if (moveInput.x != 0) lastMoveInput.x = moveInput.x; // Save last sprite direction
        }
        else // if we stand still
        {
            animator.SetBool(running, false);
            if (lastMoveInput.x != 0) animator.SetFloat(lastFaced, lastMoveInput.x);
        }
    }

    private void WandRotation(Vector2 shootInput)
    {
        if (shootInput.magnitude < 0.1f) return;
        if (shootInput.y < 0) wandSprite.sortingOrder = 1;
        else wandSprite.sortingOrder = -1;

        if (Math.Abs(shootInput.y) >= Math.Abs(shootInput.x))
            shootInput.x = 0;
        else if (Math.Abs(shootInput.x) > Math.Abs(shootInput.y))
            shootInput.y = 0;
 
        float angle = Mathf.Atan2(shootInput.y, shootInput.x) * Mathf.Rad2Deg;
        wand.MoveRotation(-90 + angle);
    }

    private void OnShooting(Vector2 shootInput, bool isShooting)
    {
        if (!isShooting) return;
        if (shootInput.magnitude < 0.1f) return;

        if (Math.Abs(shootInput.y) >= Math.Abs(shootInput.x))
        {
            shootInput.x = 0;
        }
        else if (Math.Abs(shootInput.x) > Mathf.Abs(shootInput.y)) shootInput.y = 0;

        if (Time.time >= lastShotTime + playerStats.CurrentShotCooldown)
        {
            lastShotTime = Time.time;

            GameObject bullet = Instantiate(bulletPrefab, wandTip.position, Quaternion.identity);
            bullet.transform.localScale = playerStats.CurrentBulletSize;

            if (bullet.TryGetComponent(out Rigidbody2D rbBullet))
            {
                rbBullet.linearVelocity = shootInput * playerStats.ShotSpeed;
            }
        }
    }

    /*public void OnMoves(InputAction.CallbackContext context)
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
    }*/

    /*public void OnShoot(InputAction.CallbackContext context)
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
    }*/
}
