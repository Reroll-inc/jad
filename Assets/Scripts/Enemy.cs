using Pathfinding;
using UnityEngine;
using UnityEngine.U2D;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string isHit = "Is Hit";
    private Rigidbody2D body;
    private AIPath ai;
    private Animator animator;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        ai = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Rigidbody2D bulletPhysics = collision.gameObject.GetComponent<Rigidbody2D>();
            animator.SetBool(isHit, true);
            if (bulletPhysics != null)
            {
                body.AddForce(bulletPhysics.linearVelocity.normalized * bulletPhysics.linearVelocity.magnitude, ForceMode2D.Impulse);
            }
        }
    }

    public void EnableAI()
    {
        animator.SetBool(isHit, false);
        body.linearVelocity = Vector2.zero;
        if (ai != null)
            ai.canMove = true;
    }

    public void DisableAI()
    {
        if (ai != null)
            ai.canMove = false;
    }
}
