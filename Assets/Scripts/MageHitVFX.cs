using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerHpManager))]
[RequireComponent(typeof(PlayerController))]
public class MageHitVFX : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Animation")]
    [SerializeField] private string isImmune = "Is Immune";
    [SerializeField] private string isHit = "Is Hit";
    [SerializeField] private string isDeadTrigger = "Is Dead";
    [SerializeField] private string isRunningBool = "Running";

    private Animator animator;
    private bool immune = false;

    private PlayerHpManager healthManager;
    private bool isDead = false;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        healthManager = GetComponent<PlayerHpManager>();

        healthManager.OnDeath.AddListener(HandleDeath);
    }

    void OnDestroy()
    {
        healthManager.OnDeath.RemoveListener(HandleDeath);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (isDead || immune || !collision.CompareTag(enemyTag)) return;

        immune = true;
        animator.SetBool(isImmune, true);
        healthManager.UpdateCurrentHealth(HealthOperation.Dec);
        animator.SetBool(isHit, true);
    }

    public void HandleDeath()
    {
        if (isDead) return;

        isDead = true;
        immune = true;

        animator.SetBool(isHit, false);
        animator.SetBool(isImmune, false);
        animator.SetBool(isRunningBool, false);

        animator.SetTrigger(isDeadTrigger);
        GetComponent<PlayerController>().enabled = false;
    }
    public void EndImmunity()
    {
        if (isDead) return;

        immune = false;
        animator.SetBool(isImmune, false);
    }
}
