using Pathfinding;
using UnityEngine;
using UnityEngine.U2D;

//[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(AIPath))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private string isHit = "Is Hit";
    [SerializeField] private string facing = "Facing";
    private Rigidbody2D body;
    private AIPath path;
    private Animator animator;
    void Start()
    {
        body = GetComponentInParent<Rigidbody2D>();
        path = GetComponentInParent<AIPath>();
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

    private void Update()
    {
        animator.SetFloat(facing, path.desiredVelocity.x);
    }


    public void EnableAI()
    {
        animator.SetBool(isHit, false);
        body.linearVelocity = Vector2.zero;
        if (path != null)
            path.canMove = true;
    }

    public void DisableAI()
    {
        if (path != null)
            path.canMove = false;
    }
}
