using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Wand : MonoBehaviour
{
    private Vector2 shootInput;
    private Rigidbody2D rbPivot;

    void Start()
    {
        rbPivot = GetComponent<Rigidbody2D>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            shootInput = context.ReadValue<Vector2>();
            float angle = Mathf.Atan2(shootInput.y, shootInput.x) * Mathf.Rad2Deg;
            rbPivot.MoveRotation(-90 + angle);
        }
    }
}
