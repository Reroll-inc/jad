using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2.0f;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string terrainTag = "Wall";

    private int bulletDamage = 1;
    private Rigidbody2D body;
    void Start()
    {
        Destroy(gameObject, lifetime);
        body = GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(enemyTag))
        {
            EnemyHpManager objectHP = collision.GetComponent<EnemyHpManager>();
            Rigidbody2D enemyBody = objectHP.GetComponent<Rigidbody2D>();

            Animator enemyAnimator = collision.GetComponent<Animator>();
            Enemy enemyScript = collision.GetComponent<Enemy>();

            // Enemy damaged
            if (objectHP != null)
            {
                if (enemyScript != null && !enemyScript.IsDying())
                {
                    objectHP.ReceiveDamage(bulletDamage);
                }
            }
  
            // Enemy knockback
            if (enemyBody != null && body != null)
            {
                enemyBody.AddForce(body.linearVelocity.normalized * body.linearVelocity.magnitude, ForceMode2D.Impulse);
            }

            if (enemyScript != null && !enemyScript.IsDying())
            {
                enemyScript.DisableAI();
            }
            if (enemyAnimator != null && !enemyScript.IsDying())
            {
                enemyAnimator.SetBool("Is Hit", true);
            }
            Destroy(gameObject);
        }
        else if(collision.CompareTag(terrainTag))
        {
            Destroy(gameObject);
        }
    }
}
