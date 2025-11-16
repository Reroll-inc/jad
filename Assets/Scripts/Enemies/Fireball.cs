using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string terrainTag = "Wall";

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(terrainTag) || collision.CompareTag(playerTag))
        {
            Destroy(gameObject);

            return;
        }
    }
}
