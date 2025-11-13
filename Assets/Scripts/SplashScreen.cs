using UnityEngine;
using UnityEngine.InputSystem;

public class SplashScreen : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionReference confirmAction;

    void OnEnable()
    {
        confirmAction.action.started += OnSubmit;
    }
    void OnDisable()
    {
        confirmAction.action.started -= OnSubmit;
    }

    void OnSubmit(InputAction.CallbackContext context)
    {
        GameManager.Instance.LoadMainMenu();
    }
}
