using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2.0f;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string terrainTag = "Wall";

    void Start()
    {
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
