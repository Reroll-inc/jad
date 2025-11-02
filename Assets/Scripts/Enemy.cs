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

    [Header("Stats")]
    [SerializeField] private int enemyDamage;

    private HpManager hpManager;
    private bool isDying = false;
    private Rigidbody2D body;
    private AIPath path;
    private Animator animator;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        hpManager = GetComponent<HpManager>();

        if (hpManager != null)
        {
            hpManager.onDeath.AddListener(StartDeathSequence);
        }
        else
        {
            Debug.LogError("El enemigo" + gameObject.name + "no tiene HpManager en su objeto padre!");
        }
    }

    void OnDestroy()
    {
        if (hpManager != null)
        {
            hpManager.onDeath.RemoveListener(StartDeathSequence);
        }
    }     
    
    private void Update()
    {
        if (path != null && path.canMove)
        {
            animator.SetFloat(facing, path.desiredVelocity.x);
        }
    }

    void StartDeathSequence()
    {
        if (isDying)
        {
            return;
        }

        isDying = true;

        DestroyEnemy();
    }

    public bool IsDying()
    {
        return isDying;
    }

    public void EnableAI()
    {
        animator.SetBool(isHit, false);

        if (isDying)
        {
            return;
        }

        if (body != null)
        {
            body.linearVelocity = Vector2.zero;
            if (path != null) {
                path.canMove = true;
            }
        }
    }

    public void DisableAI()
    {
        if (path != null)
            path.canMove = false;
    }

    public void DestroyEnemy()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.EnemyDefeated();
        }

        Destroy(gameObject);
    }
}
