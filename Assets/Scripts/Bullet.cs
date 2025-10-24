using Pathfinding;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2.0f;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string terrainTag = "Wall";
    private Rigidbody2D bullet;
    void Start()
    {
        bullet = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(enemyTag))
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag(terrainTag))
        {
            Destroy(gameObject);
        }
    }
}
