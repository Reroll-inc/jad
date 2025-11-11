using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
// [RequireComponent(typeof(AudioManager))] Create and add AudioManager.cs

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Instantiate GameManager on Boot
    // public AudioManager Audio {  get; private set; }
    public PlayerInput playerInput;

    [Header("Scene Indexes")] // Adding index as reference for currentSceneIndex.
    // Serialized indexes just in case we change index scene order in edit window.
    [SerializeField] private int splashScreenIndex = 1;
    [SerializeField] private int mainMenuIndex = 2;
    [SerializeField] private int creditsIndex = 3;
    [SerializeField] private int firstLevelIndex = 4;
    [SerializeField] private int lastLevelIndex = 23; // if Index = 23, NextScene = Credits.


    [Header("Input Action Maps")]
    [SerializeField] private string uiActionMap = "UI";
    [SerializeField] private string gameplayActionMap = "Gameplay";

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseMenuPanel; // Drag pause panel here

    private bool isPaused = false;
    private int currentSceneIndex;

    void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerInput = GetComponent<PlayerInput>();
        // audio = GetComponent<AudioManager>(); // Add AudioManager
    }

    void Start()
    {
        SceneManager.LoadSceneAsync(splashScreenIndex, LoadSceneMode.Additive);
        playerInput.SwitchCurrentActionMap(uiActionMap);
        currentSceneIndex = splashScreenIndex;
    }
    public void LoadMainMenu()
    {
        SceneManager.UnloadSceneAsync(currentSceneIndex); // It does not matter what scene was previous
        SceneManager.LoadSceneAsync(mainMenuIndex, LoadSceneMode.Additive);
        playerInput.SwitchCurrentActionMap(uiActionMap);
        currentSceneIndex = mainMenuIndex;
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
        playerInput.SwitchCurrentActionMap(uiActionMap);
        currentSceneIndex = creditsIndex;
    }

    public void LoadNextLevel()
    {
        int nextIndex = currentSceneIndex + 1;
        if (nextIndex > lastLevelIndex)
        {
            Debug.Log("Game finished! Showing credits"); // Check if went to credits correctly
            LoadCredits();
        }
        else
        {
            Debug.Log($"Loading Level {nextIndex}"); // Check if level loaded correctly
            SceneManager.UnloadSceneAsync(currentSceneIndex);
            SceneManager.LoadSceneAsync(nextIndex, LoadSceneMode.Additive);
            playerInput.SwitchCurrentActionMap(gameplayActionMap);
            currentSceneIndex = nextIndex;
        }
    }

    // Enter In-Game Pause Menu
    public void TogglePause()
    {
        isPaused = !isPaused; // Inverted state
        if (isPaused) PauseGame();
        else ResumeGame();
    }

    // Pause Logic
    private void PauseGame()
    {
        Debug.Log("Game Paused"); // Check if paused correctly
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        playerInput.SwitchCurrentActionMap(uiActionMap);
    }

    private void ResumeGame()
    {
        Debug.Log("Game resumed"); // Check if resumed correctly
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        playerInput.SwitchCurrentActionMap(gameplayActionMap);
    }

    // Go back to Main Menu
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        LoadMainMenu();
    }

    // Quit Game
    public void QuitGame()
    {
        Debug.Log("Closing game..."); // Check if quitting correctly
        Application.Quit();
    }
}

// Lots of text, sry xd
