using UnityEngine;
using UnityEngine.InputSystem;

public class SplashScreen : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private string confirmActionName = "Submit"; // Detects any key

    private InputAction confirmAction;
    void Start()
    {
        PlayerInput playerInput = GameManager.Instance.playerInput;
        InputActionMap uiMap = playerInput.actions.FindActionMap("UI");
        confirmAction = uiMap.FindAction(confirmActionName);
    }

    void Update()
    {
        if (confirmAction.WasPressedThisFrame())
        {
            GameManager.Instance.LoadMainMenu();
            this.enabled = false;
        }
    }
}
