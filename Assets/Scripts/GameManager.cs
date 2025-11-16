using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum GameInputMap { UI, Gameplay }

// [RequireComponent(typeof(AudioManager))] Create and add AudioManager.cs
// Navigation System -- input system -- llevar track de la navegación del previo botón activo -- botón de salida a la derecha
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // public AudioManager Audio {  get; private set; }

    [Header("Scene Indexes")]
    [SerializeField] private int splashScreenIndex = 1;
    [SerializeField] private int mainMenuIndex = 2;
    [SerializeField] private int creditsIndex = 3;
    [SerializeField] private int firstLevelIndex = 4;
    [Tooltip("If the index is lastLevelIndex, next scene goes to credits")]
    [SerializeField] private int lastLevelIndex = 23;
    [Tooltip("DEV_ONLY. If set, we jump to that level")]
    [SerializeField] private int devInitialLevel = -1;

    [Header("Input Action Maps")]
    [SerializeField] private string uiActionMap = "UI";
    [SerializeField] private string gameplayActionMap = "Gameplay";
    [SerializeField] private string splashActionMap = "Splash";

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseMenuPanel;

    private bool isPaused = false;
    private int currentSceneIndex;

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        // audio = GetComponent<AudioManager>(); // Add AudioManager

        if (devInitialLevel == -1)
        {
            ToggleSplashKeymap(true);
            SceneManager.LoadSceneAsync(splashScreenIndex, LoadSceneMode.Additive);
            currentSceneIndex = splashScreenIndex;
        }
        else
        {
            ActivateActionMap(GameInputMap.UI);
            DEV_LoadLevel();
        }
    }

    void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);

        ActivateActionMap(GameInputMap.UI);
    }

    void ResumeGame()
    {
        Debug.Log("Game resumed");
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);

        ActivateActionMap(GameInputMap.Gameplay);
    }

    void DEV_LoadLevel()
    {
        if (devInitialLevel == -1) return;

        SceneManager.LoadSceneAsync(firstLevelIndex + devInitialLevel - 1, LoadSceneMode.Additive);
        currentSceneIndex = firstLevelIndex + devInitialLevel;
    }

    public void ActivateActionMap(GameInputMap actionMapName)
    {
        switch (actionMapName)
        {
            case GameInputMap.UI:
                InputSystem.actions.FindActionMap(gameplayActionMap).Disable();
                InputSystem.actions.FindActionMap(uiActionMap).Enable();
                break;
            case GameInputMap.Gameplay:
                InputSystem.actions.FindActionMap(uiActionMap).Disable();
                InputSystem.actions.FindActionMap(gameplayActionMap).Enable();
                break;
        }
    }

    void ToggleSplashKeymap(bool activate)
    {
        if (activate)
        {
            InputSystem.actions.FindActionMap(splashActionMap).Enable();
            InputSystem.actions.FindActionMap(uiActionMap).Disable();
            InputSystem.actions.FindActionMap(gameplayActionMap).Disable();
        }
        else
        {
            InputSystem.actions.FindActionMap(splashActionMap).Disable();
            InputSystem.actions.FindActionMap(uiActionMap).Enable();
            InputSystem.actions.FindActionMap(gameplayActionMap).Disable();
        }
    }

    public void LoadMainMenu()
    {
        ToggleSplashKeymap(false);
        SceneManager.UnloadSceneAsync(currentSceneIndex);
        SceneManager.LoadSceneAsync(mainMenuIndex, LoadSceneMode.Additive);
        currentSceneIndex = mainMenuIndex;
    }

    public void StartGame()
    {
        SceneManager.UnloadSceneAsync(currentSceneIndex);
        SceneManager.LoadSceneAsync(firstLevelIndex, LoadSceneMode.Additive);
        currentSceneIndex = firstLevelIndex;
        Debug.Log($"Current Index = {currentSceneIndex}");
    }

    public void LoadCredits()
    {
        SceneManager.UnloadSceneAsync(currentSceneIndex);
        SceneManager.LoadSceneAsync(creditsIndex, LoadSceneMode.Additive);
        InputSystem.actions.FindActionMap(uiActionMap).Enable();
        currentSceneIndex = creditsIndex;
    }

    public void LoadNextLevel()
    {
        int nextIndex = currentSceneIndex + 1;

        if (nextIndex > lastLevelIndex)
        {
            Debug.Log("Game finished! Showing credits");
            LoadCredits();
        }
        else
        {
            Debug.Log($"Loading Level {nextIndex}");            
            SceneManager.UnloadSceneAsync(currentSceneIndex);
            SceneManager.LoadSceneAsync(nextIndex, LoadSceneMode.Additive);
            currentSceneIndex = nextIndex;
            Debug.Log($"Current Index = {currentSceneIndex}");
            //ActivateActionMap(GameInputMap.Gameplay);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused) PauseGame();
        else ResumeGame();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        LoadMainMenu();
    }

    public void QuitGame()
    {
        Debug.Log("Closing game...");
        Application.Quit();
    }
}
