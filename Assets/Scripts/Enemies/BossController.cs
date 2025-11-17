using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BossStats))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossController : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds1 = new(1f);
    [SerializeField] private string mouthNode = "Mouth";
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string bossPortalTag = "Boss Portal";
    [SerializeField] private GameObject fireballPrefab;

    private Transform mouth;
    private Transform playerTransform;
    private BossStats stats;
    private Rigidbody2D body;
    private Transform[] teleporters;
    private EnemyHpManager bossHpManager;

    private int portalIndex = 1;
    private bool isDying = false;

    private Coroutine throwFireballCoroutine;
    private Coroutine teletransportCoroutine;

    void Awake()
    {
        stats = GetComponent<BossStats>();
        body = GetComponent<Rigidbody2D>();
        bossHpManager = GetComponent<EnemyHpManager>();
        bossHpManager.onDeath.AddListener(StartDeathSequence);
        mouth = transform.Find(mouthNode);
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();
        teleporters = GameObject
            .FindGameObjectsWithTag(bossPortalTag)
            .Select(bossPortal => bossPortal.transform)
            .ToArray();
    }

    void Start()
    {

        throwFireballCoroutine = StartCoroutine(ThrowFireball());
        teletransportCoroutine = StartCoroutine(Teletransport());
        portalIndex = CalculateShorterTP();
    }

    void OnDestroy()
    {
        StopCoroutine(throwFireballCoroutine);
        StopCoroutine(teletransportCoroutine);
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
    void StartDeathSequence()
    {
        if (isDying) return;

        isDying = true;

        DestroyEnemy();
    }

    IEnumerator Teletransport()
    {
        while (true)
        {
            yield return _waitForSeconds1;

            int nextIndex = CalculateShorterTP();

            if (nextIndex != portalIndex)
            {
                portalIndex = nextIndex;

                body.position = teleporters[portalIndex].position;

                stats.TeleportPenalization();

                yield return new WaitForSeconds(stats.TeletransportCD);
            }
        }
    }

    int CalculateShorterTP()
    {

        int nextIndex = portalIndex;
        Vector3 playerPosition = playerTransform.position;
        float nextDistance = (playerPosition - teleporters[nextIndex].position).magnitude;

        for (int i = 0; i < teleporters.Length; i++)
        {
            if (nextIndex == i) continue;

            float distance = (playerPosition - teleporters[i].position).magnitude;

            if (distance < nextDistance)
            {
                nextIndex = i;
                nextDistance = distance;
            }
        }

        return nextIndex;
    }

    public void DestroyEnemy()
    {
        LevelManager.Instance.OnEnemyDefeated.Invoke();

        Destroy(gameObject);
    }
}
