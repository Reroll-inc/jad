using UnityEngine;
using UnityEngine.InputSystem;

public class MageHitVFX : MonoBehaviour
{
    [SerializeField] private string isImmune = "Is Immune";
    [SerializeField] private string isHit = "Is Hit";
    [SerializeField] private string isDeadTrigger = "Is Dead";
    [SerializeField] private string isRunningBool = "Running";

    private Animator animator;
    private bool immune = false;

    private HealthManager healthManager;
    private bool isDead = false; // Avoids multiple IsDead calls
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        healthManager = GetComponent<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager not found on player");
        }
        else
        {
            healthManager.OnDeath.AddListener(HandleDeath);
        }
    }

    void OnDestroy()
    {
        if (healthManager != null)
        {
            healthManager.OnDeath.RemoveListener(HandleDeath);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            if (!immune)
            {
                immune = true;       
                animator.SetBool(isImmune, true);

                healthManager.UpdateCurrentHealth(HealthOperation.Dec); //Apply player HP decrease
                if (!isDead)
                {
                    animator.SetBool(isHit, true);
                }
            }
        }
    }

    public void HandleDeath()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        immune = true;

        animator.SetBool(isHit, false);
        animator.SetBool(isImmune, false);
        animator.SetBool(isRunningBool, false);

        animator.SetTrigger(isDeadTrigger);
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
    }
    public void EndImmunity()
    {
        if (isDead)
        {
            return;
        }

        immune = false;
        animator.SetBool(isImmune, false);
    }
}
