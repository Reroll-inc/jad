using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("HUD reference")]
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private TextMeshProUGUI enemyCounterText;

    [SerializeField] private GameObject screenLevelComplete;
    [SerializeField] private GameObject screenGameOver;

    [Header("Card Selection")]
    [SerializeField] private CardScreenController cardScreenController;

    [Header("Player reference")]
    public PlayerStats playerStats;

    public UnityEvent OnEnemyDefeated;

    private int remainingEnemies;

    void Start()
    {
        OnEnemyDefeated.AddListener(EnemyDefeated);

        screenLevelComplete.SetActive(false);
        screenGameOver.SetActive(false);
        hudCanvas.SetActive(false);
        cardScreenController.ShowCardSelection();
    }

    void OnDestroy()
    {
        OnEnemyDefeated.RemoveListener(EnemyDefeated);
    }

    void EnemyDefeated()
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

    public void StartLevel()
    {
        hudCanvas.SetActive(true);

        Time.timeScale = 1f;
        remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        UpdateHUD();
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
        screenGameOver.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CardSelect(CardType cardType)
    {
        playerStats.ApplyPowerUp(cardType);
        cardScreenController.HideCardSelection();

        StartLevel();
    }

    public static LevelManager GetComponent()
    {
        string tag = "LevelManager";

        if (!GameObject.FindGameObjectWithTag(tag).TryGetComponent(out LevelManager levelManager))
        {
            Debug.LogWarning("Node with tag " + tag + " doesn't have a LevelManager component.");
        }

        return levelManager;
    }
}
