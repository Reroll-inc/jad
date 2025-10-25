using UnityEngine;

public class MageHitVFX : MonoBehaviour
{
    [SerializeField] private string isImmune = "Is Immune";
    [SerializeField] private string isHit = "Is Hit";
    private Animator animator;
    private bool immune = false;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!immune)
            {
                immune = true;
                animator.SetBool(isHit, true);
                animator.SetBool(isImmune, true);
            }
        }
    }

    public void EndImmunity()
    {
        immune = false;
        animator.SetBool(isImmune, false);
    }
}
