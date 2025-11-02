using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManagement : MonoBehaviour
{
    [SerializeField] private string isHit = "Is Hit";

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void NotifyEndImmunity()
    {
        GetComponentInParent<MageHitVFX>()?.EndImmunity();
    }

    public void EndHitAnimation()
    {
        animator.SetBool(isHit, false);
    }

    public void ShowGameOverScreen()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ActivateGameOver();
        }

    }
}
