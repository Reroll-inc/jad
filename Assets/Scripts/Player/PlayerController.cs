using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerHpManager))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Animation States")]
    [SerializeField] private string running = "Running";
    [SerializeField] private string facing = "CurrentInput";
    [SerializeField] private string lastFaced = "LastInput";

    [Header("Inputs")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference attack;

    private Rigidbody2D body;
    private Animator animator;
    private Vector2 playerInput;
    private PlayerStats playerStats;
    private Wand weapon;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStats>();
        weapon = GetComponentInChildren<Wand>();
    }

    void OnEnable()
    {
        move.action.performed += OnMove;
        move.action.canceled += OnCancelMove;

        attack.action.performed += weapon.OnShoot;
        attack.action.canceled += weapon.OnShoot;
    }

    void OnDisable()
    {
        move.action.performed -= OnMove;
        move.action.canceled -= OnCancelMove;

        attack.action.performed -= weapon.OnShoot;
        attack.action.canceled -= weapon.OnShoot;
    }

    void OnCancelMove(InputAction.CallbackContext context)
    {
        body.linearVelocity = new(0f, 0f);
        animator.SetBool(running, false);

        if (playerInput.x != 0)
        {
            animator.SetFloat(lastFaced, playerInput.x);
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        animator.SetBool(running, true);

        playerInput = context.ReadValue<Vector2>();

        animator.SetFloat(facing, playerInput.x);

        body.linearVelocity = playerStats.MoveSpeed * playerInput;
    }
}
