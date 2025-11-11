using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerHpManager))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Animation States")]
    [SerializeField] private string running = "Running";
    [SerializeField] private string facing = "CurrentInput";
    [SerializeField] private string lastFaced = "LastInput";

    private Rigidbody2D body;
    private Animator animator;
    private Vector2 playerInput;
    private PlayerStats playerStats;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStats>();
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

        body.linearVelocity = playerStats.MoveSpeed * playerInput;
    }
}
