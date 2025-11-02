using UnityEngine;

public class MageHitVFX : MonoBehaviour
{
    [SerializeField] private string isImmune = "Is Immune";
    [SerializeField] private string isHit = "Is Hit";
    [SerializeField] private string isDeadTrigger = "Is Dead";

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
                animator.SetBool(isHit, true);
                animator.SetBool(isImmune, true);

                healthManager.UpdateCurrentHealth(HealthOperation.Dec); //Apply player HP decrease
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

        animator.SetTrigger(isDeadTrigger);
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
