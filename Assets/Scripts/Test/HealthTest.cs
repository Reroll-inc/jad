using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerHpManager))]
public class HealthTest : MonoBehaviour
{
    private PlayerHpManager healthManager;

    void Start()
    {
        healthManager = GetComponent<PlayerHpManager>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            healthManager.UpdateCurrentHealth(HealthOperation.Inc);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            healthManager.UpdateCurrentHealth(HealthOperation.Dec);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            healthManager.IncrementMaxHealth();
    }
}
