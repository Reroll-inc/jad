using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2.0f;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string terrainTag = "Wall";
    [SerializeField] private string fireballTag = "Boss Fireball";

    private readonly int bulletDamage = 1;
    private Rigidbody2D body;

    void Start()
    {
        Destroy(gameObject, lifetime);
        body = GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(terrainTag) || collision.CompareTag(fireballTag))
        {
            Destroy(gameObject);

            return;
        }
        if (!collision.CompareTag(enemyTag)) return;

        Enemy enemy = collision.GetComponentInParent<Enemy>();

        enemy.ReceiveDamage(bulletDamage, body.linearVelocity.normalized * body.linearVelocity.magnitude);

        Destroy(gameObject);
    }
}
