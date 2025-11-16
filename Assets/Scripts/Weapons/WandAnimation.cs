using UnityEngine;

public class WandAnimation : MonoBehaviour
{
    [SerializeField] private string isHit = "Is Hit";

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void HitEnd()
    {
        animator.SetBool(isHit, false);
    }
}
