using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

enum HandleToggle
{
    ON, OFF
}
enum HandleAction
{
    MOVE, ATTACK, DASH
}


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

    private bool dashing = false;
    private bool attacking = false;

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
        HandleActionEvents(HandleAction.ATTACK, HandleToggle.ON);
        HandleActionEvents(HandleAction.MOVE, HandleToggle.ON);
        HandleActionEvents(HandleAction.DASH, HandleToggle.ON);
    }

    void OnDisable()
    {
        HandleActionEvents(HandleAction.ATTACK, HandleToggle.OFF);
        HandleActionEvents(HandleAction.MOVE, HandleToggle.OFF);
        HandleActionEvents(HandleAction.DASH, HandleToggle.OFF);
    }

    void HandleActionEvents(HandleAction action, HandleToggle handle)
    {
        switch (action)
        {
            case HandleAction.ATTACK:
                switch (handle)
                {
                    case HandleToggle.ON:
                        attack.action.performed += weapon.OnShoot;
                        attack.action.canceled += weapon.OnShoot;
                        attack.action.performed += OnShoot;
                        attack.action.canceled += OnShoot;
                        break;
                    case HandleToggle.OFF:
                        attack.action.performed -= weapon.OnShoot;
                        attack.action.canceled -= weapon.OnShoot;
                        attack.action.performed -= OnShoot;
                        attack.action.canceled -= OnShoot;
                        break;
                }
                break;
            case HandleAction.MOVE:
                switch (handle)
                {
                    case HandleToggle.ON:
                        move.action.performed += OnMove;
                        move.action.canceled += OnCancelMove;
                        break;
                    case HandleToggle.OFF:
                        move.action.performed -= OnMove;
                        move.action.canceled -= OnCancelMove;
                        break;
                }
                break;
            case HandleAction.DASH:
                switch (handle)
                {
                    case HandleToggle.ON:
                        dash.action.started += OnDash;
                        break;
                    case HandleToggle.OFF:
                        dash.action.started -= OnDash;
                        break;
                }
                break;
        }
    }

    void OnCancelMove(InputAction.CallbackContext context)
    {
        if (dashing)
        {
            playerInput = new(0f, 0f);
            return;
        }

        body.linearVelocity = new(0f, 0f);
        animator.SetBool(running, false);

        if (playerInput.x != 0)
        {
            animator.SetFloat(lastFaced, playerInput.x);
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>() * (attacking ? playerStats.MovePenalization : 1);

        if (dashing) return;
        animator.SetBool(running, true);
        animator.SetFloat(facing, playerInput.x);

        body.linearVelocity = playerStats.MoveSpeed * playerInput;
    }

    void OnDash(InputAction.CallbackContext context)
    {
        if (body.linearVelocity.x != 0 || body.linearVelocity.y != 0)
        {
            HandleActionEvents(HandleAction.DASH, HandleToggle.OFF);

            StartCoroutine(DashCoroutine());
        }
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            attacking = false;
            HandleActionEvents(HandleAction.DASH, HandleToggle.ON);

            return;
        }
        if (context.performed)
        {
            attacking = true;
            HandleActionEvents(HandleAction.DASH, HandleToggle.OFF);
        }
    }

    IEnumerator DashCoroutine()
    {
        dashing = true;
        mageHitVFX.StartImmunity();

        body.linearVelocity = body.linearVelocity.normalized * playerStats.DashVelocity;

        yield return new WaitForSeconds(playerStats.DashDuration);

        dashing = false;
        body.linearVelocity = playerStats.MoveSpeed * playerInput;

        mageHitVFX.EndImmunity();

        yield return new WaitForSeconds(playerStats.DashCooldown);

        HandleActionEvents(HandleAction.DASH, HandleToggle.ON);
    }
}
