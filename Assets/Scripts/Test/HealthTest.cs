using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HealthManager))]
public class HealthTest : MonoBehaviour
{
    private HealthManager healthManager;

    void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            healthManager.UpdateCurrentHealth(HealthOperation.Inc);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            healthManager.UpdateCurrentHealth(HealthOperation.Dec);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            healthManager.IncrementMaxHealth();
        }
    }
}
