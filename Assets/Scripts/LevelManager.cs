using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("HUD reference")]
    public TextMeshProUGUI enemyCounterText;

    [SerializeField] private GameObject screenLevelComplete;

    [Header("Player reference")]
    public PlayerController player;

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
        Time.timeScale = 1f;

        screenLevelComplete.SetActive(false);

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
    }

    /*public void CardSelect(CardType cardType)
    {
        if (player != null)
        {
            player.ApllyPowerUp(cardType);
        }

        Time.timeScale = 1f;

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }*/
}
