using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("HUD reference")]
    public TextMeshProUGUI enemyCounterText;

    [SerializeField] private GameObject screenLevelComplete;
    [SerializeField] private GameObject screenGameOver;

    [Header("Card Selection")]
    [SerializeField] private CardScreenController cardScreenController;

    [Header("Player reference")]
    public PlayerStats playerStats;

    private int remainingEnemies;

    void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {

        screenLevelComplete.SetActive(false);

        if (screenGameOver != null)
        {
            screenGameOver.SetActive(false);
        }

        if (cardScreenController != null)
        {
            cardScreenController.ShowCardSelection();
        }
        else
        {
            StartLevel();
        }      
    }
    
    public void StartLevel()
    {
        Time.timeScale = 1f; //start level
        remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        UpdateHUD();
    }

    public void EnemyDefeated()
    {
        remainingEnemies--;
        UpdateHUD();

        if (remainingEnemies == 0)
        {
            CompleteLevel();
        }
    }

    void UpdateHUD()
    {
        enemyCounterText.text = remainingEnemies.ToString();
    }

    void CompleteLevel()
    {
        screenLevelComplete.SetActive(true);
        Time.timeScale = 0f;
        //GoToNextLevel() goes into screenLevelComplete(Unity UI)
    }

    public void GoToNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void ActivateGameOver()
    {
        if (screenGameOver != null)
        {
            screenGameOver.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void CardSelect(CardType cardType)
    {
        if (playerStats != null)
        {
            playerStats.ApplyPowerUp(cardType);
        }

        if (cardScreenController != null)
        {
            cardScreenController.HideCardSelection();
        }

        StartLevel();
    }
}
