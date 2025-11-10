using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManagement : MonoBehaviour
{
    [SerializeField] private string isHit = "Is Hit";

    private MageHitVFX mageHit;
    private Animator animator;
    private LevelManager levelManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        levelManager = LevelManager.GetComponent();
        mageHit = GetComponentInParent<MageHitVFX>();
    }
    public void NotifyEndImmunity()
    {
        mageHit.EndImmunity();
    }

    public void EndHitAnimation()
    {
        animator.SetBool(isHit, false);
    }

    public void ShowGameOverScreen()
    {
        levelManager.ActivateGameOver();
    }
}
