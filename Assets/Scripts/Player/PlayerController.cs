using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

enum HandleToggle
{
    ON, OFF
}
enum HandleAction
{
    MOVE, ATTACK, DASH, PAUSE
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerHpManager))]
[RequireComponent(typeof(MageHitVFX))]
[RequireComponent(typeof(AudioSource))]
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
    [SerializeField] private InputActionReference pause;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip dashClip;

    [Header("Sound Variation")]
    [SerializeField, Range(0.1f, 3f)] private float minPitch = 0.9f;
    [SerializeField, Range(0.1f, 3f)] private float maxPitch = 1.1f;

    private Rigidbody2D body;
    private Vector2 playerInput;
    private Animator animator;
    private MageHitVFX mageHitVFX;
    private Wand weapon;
    private AudioSource audioSource;

    private bool dashing = false;
    private bool attacking = false;

    private Coroutine cancelAttackCoroutineId;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        mageHitVFX = GetComponent<MageHitVFX>();
        weapon = GetComponentInChildren<Wand>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        HandleActionEvents(HandleAction.ATTACK, HandleToggle.ON);
        HandleActionEvents(HandleAction.MOVE, HandleToggle.ON);
        HandleActionEvents(HandleAction.DASH, HandleToggle.ON);
        HandleActionEvents(HandleAction.PAUSE, HandleToggle.ON);
    }

    void OnDisable()
    {
        HandleActionEvents(HandleAction.ATTACK, HandleToggle.OFF);
        HandleActionEvents(HandleAction.MOVE, HandleToggle.OFF);
        HandleActionEvents(HandleAction.DASH, HandleToggle.OFF);
        HandleActionEvents(HandleAction.PAUSE, HandleToggle.OFF);
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
            case HandleAction.PAUSE:
                switch (handle)
                {
                    case HandleToggle.ON:
                        pause.action.performed += OnPause;
                        break;
                    case HandleToggle.OFF:
                        pause.action.performed -= OnPause;
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
        playerInput = context.ReadValue<Vector2>();

        if (attacking)
            playerInput *= GameManager.Instance.playerStats.MovePenalization;

        if (dashing) return;

        animator.SetBool(running, true);
        animator.SetFloat(facing, playerInput.x);

        body.linearVelocity = GameManager.Instance.playerStats.MoveSpeed * playerInput;
    }

    void OnDash(InputAction.CallbackContext context)
    {
        if (!attacking && (body.linearVelocity.x != 0 || body.linearVelocity.y != 0))
        {
            HandleActionEvents(HandleAction.DASH, HandleToggle.OFF);

            StartCoroutine(DashCoroutine());
        }
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            cancelAttackCoroutineId = StartCoroutine(CancelAttackCoroutine());

            return;
        }
        if (context.performed)
        {
            if (cancelAttackCoroutineId != null)
                StopCoroutine(cancelAttackCoroutineId);

            if (!attacking)
                body.linearVelocity *= GameManager.Instance.playerStats.MovePenalization;

            attacking = true;
            audioSource.pitch = Random.Range(minPitch, maxPitch);

            audioSource.PlayOneShot(shootClip);
        }
    }
    void OnPause(InputAction.CallbackContext context)
    {
        LevelManager.Instance.PauseGame();
    }

    IEnumerator CancelAttackCoroutine()
    {
        yield return new WaitForSeconds(GameManager.Instance.playerStats.AttackPenalization);

        attacking = false;
    }

    IEnumerator DashCoroutine()
    {
        dashing = true;
        mageHitVFX.StartImmunity();
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(dashClip);

        body.linearVelocity = body.linearVelocity.normalized * GameManager.Instance.playerStats.DashVelocity;

        yield return new WaitForSeconds(GameManager.Instance.playerStats.DashDuration);

        dashing = false;

        body.linearVelocity = GameManager.Instance.playerStats.MoveSpeed * playerInput;

        mageHitVFX.EndImmunity();

        yield return new WaitForSeconds(GameManager.Instance.playerStats.DashCooldown);

        HandleActionEvents(HandleAction.DASH, HandleToggle.ON);
    }

    public void OnDead()
    {
        enabled = false;
        body.linearVelocity = new(0f, 0f);
    }
}
