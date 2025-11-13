using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

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

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogError("Two instances of LevelManager exists shouldn't exist in the same Scene.");

            return;
        }

        Instance = this;
    }


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
        GameManager.Instance.ActivateActionMap(GameInputMap.Gameplay);
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
        GameManager.Instance.ActivateActionMap(GameInputMap.UI);
    }

    public void CardSelect(CardType cardType)
    {
        playerStats.ApplyPowerUp(cardType);
        cardScreenController.HideCardSelection();

        StartLevel();
    }
}
