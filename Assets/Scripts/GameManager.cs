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
    [Tooltip("If Index = 23, NextScene = Credits")]
    [SerializeField] private int lastLevelIndex = 23;

    [Header("Input Action Maps")]
    [SerializeField] private string uiActionMap = "UI";
    [SerializeField] private string gameplayActionMap = "Gameplay";

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
        SceneManager.LoadSceneAsync(splashScreenIndex, LoadSceneMode.Additive);
        currentSceneIndex = splashScreenIndex;

        ActivateActionMap(GameInputMap.UI);
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

    public void LoadMainMenu()
    {
        SceneManager.UnloadSceneAsync(currentSceneIndex);
        SceneManager.LoadSceneAsync(mainMenuIndex, LoadSceneMode.Additive);
        currentSceneIndex = mainMenuIndex;

        ActivateActionMap(GameInputMap.UI);
    }

    public void StartGame()
    {
        SceneManager.UnloadSceneAsync(currentSceneIndex);
        SceneManager.LoadSceneAsync(firstLevelIndex, LoadSceneMode.Additive);
        currentSceneIndex = firstLevelIndex;
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

            ActivateActionMap(GameInputMap.Gameplay);
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
