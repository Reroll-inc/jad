using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(EnemyHpManager))]
[RequireComponent(typeof(AIDestinationSetter))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [Header("Events")]
    [SerializeField] private string isHit = "Is Hit";
    [SerializeField] private string facing = "Facing";

    [Header("Stats")]
    [SerializeField] private int enemyDamage;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;

    [Header("Sound Variation")]
    [SerializeField, Range(0.1f, 3f)] private float minPitch = 0.9f;
    [SerializeField, Range(0.1f, 3f)] private float maxPitch = 1.1f;

    private bool isDying = false;
    private EnemyHpManager hpManager;
    private Rigidbody2D body;
    private AIPath path;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        hpManager = GetComponent<EnemyHpManager>();
        audioSource = GetComponent<AudioSource>();

        hpManager.onDeath.AddListener(StartDeathSequence);

        GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();
    }

    void OnDestroy()
    {
        hpManager.onDeath.RemoveListener(StartDeathSequence);
    }

    void Update()
    {
        if (path.canMove)
        {
            animator.SetFloat(facing, path.desiredVelocity.x);
        }
    }

    void StartDeathSequence()
    {
        if (isDying) return;
        isDying = true;
        // Everything below is for reproducing dead sound while mob is dying.
        path.canMove = false;
        body.linearVelocity = Vector2.zero;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        path.enabled = false;
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(deathClip, 1.5f);

        DestroyEnemy();
    }

    public void ReceiveDamage(int damage)
    {
        if (isDying) return;
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(hitClip, 1.5f);
        hpManager.ReceiveDamage(damage);
        animator.SetBool(isHit, true);
    }

    public void EnableAI()
    {
        animator.SetBool(isHit, false);

        if (isDying) return;

        body.linearVelocity = Vector2.zero;
        path.canMove = true;
    }

    public void DestroyEnemy()
    {
        LevelManager.Instance.OnEnemyDefeated.Invoke();
        float delay = 0f;
        delay = deathClip.length;
        Destroy(gameObject, delay);
    }
}
