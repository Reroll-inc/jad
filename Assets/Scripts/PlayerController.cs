using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float shotSpeed = 10.0f;
    [SerializeField] private float shotCooldown = 0.5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private string running = "Running";
    [SerializeField] private string facing = "CurrentInput";
    [SerializeField] private string lastFaced = "LastInput";
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 playerInput;
    private Vector2 shootInput;
    private bool shooting = false;
    private float lastShotTime = -Mathf.Infinity;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
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
        body.linearVelocity = playerInput * moveSpeed;
        animator.SetFloat(facing, playerInput.x);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        shooting = true;
        shootInput = context.ReadValue<Vector2>();
        if (Math.Abs(shootInput.y) >= Math.Abs(shootInput.x))
        {
            shootInput.x = 0;
        }
        if (context.canceled)
        {
            shooting = false;
        }
    }

    public void Update()
    {
        if (Time.time >= lastShotTime + shotCooldown && shooting)
        {
            GameObject bullet = Instantiate(bulletPrefab, body.position, Quaternion.identity);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            if (rbBullet != null)
            {
                rbBullet.linearVelocity = shootInput * shotSpeed;
            }
            lastShotTime = Time.time;
        }
    }
}
