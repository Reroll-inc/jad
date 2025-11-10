using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(EnemyHpManager))]
[RequireComponent(typeof(AIDestinationSetter))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private string playerTag;

    [Header("Events")]
    [SerializeField] private string isHit = "Is Hit";
    [SerializeField] private string facing = "Facing";

    [Header("Stats")]
    [SerializeField] private int enemyDamage;

    private bool isDying = false;
    private EnemyHpManager hpManager;
    private Rigidbody2D body;
    private AIPath path;
    private Animator animator;
    private LevelManager levelManager;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        hpManager = GetComponent<EnemyHpManager>();
        levelManager = LevelManager.GetComponent();

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

        DestroyEnemy();
    }

    public void ReceiveDamage(int damage, Vector2 forceOfImpact)
    {
        if (isDying) return;

        hpManager.ReceiveDamage(damage);
        path.canMove = false;
        animator.SetBool(isHit, true);

        body.AddForce(forceOfImpact, ForceMode2D.Impulse);
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
        levelManager.OnEnemyDefeated.Invoke();

        Destroy(gameObject);
    }
}
