using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BossStats))]
public class BossController : MonoBehaviour
{

    [SerializeField] private string mouthNode = "Mouth";
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private GameObject fireballPrefab;

    private Transform mouth;
    private Transform playerTransform;
    private BossStats stats;

    void Start()
    {
        stats = GetComponent<BossStats>();
        mouth = transform.Find(mouthNode);
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();

        StartCoroutine(ThrowFireball());
    }

    IEnumerator ThrowFireball()
    {
        while (true)
        {
            yield return new WaitForSeconds(stats.FireballCD);

            GameObject fireball = Instantiate(fireballPrefab, mouth.position, Quaternion.identity);

            fireball.transform.SetParent(mouth);

            var heading = playerTransform.position - mouth.position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            var fireballBody = fireball.GetComponent<Rigidbody2D>();

            fireballBody.linearVelocity = direction * stats.FireballSpeed;

        }
    }
}
