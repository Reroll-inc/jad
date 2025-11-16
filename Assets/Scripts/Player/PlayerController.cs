using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerHpManager))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(MageHitVFX))]
public class PlayerController : MonoBehaviour
{
    [Header("Animation States")]
    [SerializeField] private string running = "Running";
    [SerializeField] private string facing = "CurrentInput";
    [SerializeField] private string lastFaced = "LastInput";

    [Header("Inputs")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference attack;
    [SerializeField] private InputActionReference dash;

    private Rigidbody2D body;
    private Vector2 playerInput;
    private Animator animator;
    private PlayerStats playerStats;
    private MageHitVFX mageHitVFX;
    private Wand weapon;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerStats = GetComponent<PlayerStats>();
        mageHitVFX = GetComponent<MageHitVFX>();
        weapon = GetComponentInChildren<Wand>();
    }

    void OnEnable()
    {
        move.action.performed += OnMove;
        move.action.canceled += OnCancelMove;

        attack.action.performed += weapon.OnShoot;
        attack.action.canceled += weapon.OnShoot;

        dash.action.performed += OnDash;
    }

    void OnDisable()
    {
        move.action.performed -= OnMove;
        move.action.canceled -= OnCancelMove;

        attack.action.performed -= weapon.OnShoot;
        attack.action.canceled -= weapon.OnShoot;

        dash.action.performed -= OnDash;
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

    void OnDash(InputAction.CallbackContext context)
    {
        if (body.linearVelocity.x != 0 || body.linearVelocity.y != 0)
        {
            dash.action.performed -= OnDash;

            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        mageHitVFX.StartImmunity();

        Vector2 baseVelocity = body.linearVelocity;

        body.linearVelocity = body.linearVelocity.normalized * playerStats.DashVelocity;

        yield return new WaitForSeconds(playerStats.DashDuration);

        body.linearVelocity = baseVelocity;

        mageHitVFX.EndImmunity();

        yield return new WaitForSeconds(playerStats.DashCooldown);

        dash.action.performed += OnDash;
    }
}
