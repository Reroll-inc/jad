using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HealthSpriteSet
{
    [Tooltip("Max health for this set (e.g.: 3, 4, 5...)")]
    public int maxHealth;

    [Tooltip("Drag the sprites here. Index 0 must be for 0 HP.")]
    public Sprite[] healthSprites;
}

[RequireComponent(typeof(Image))]
public class PlayerHeal : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [Header("Health Sprite Configuration")]
    [SerializeField] private List<HealthSpriteSet> spriteSets;

    private Image image;
    private PlayerHpManager healthManager;

    private readonly Dictionary<int, Sprite[]> sprites = new();

    void Start()
    {
        image = GetComponent<Image>();

        healthManager = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerHpManager>();

        healthManager.OnHealthChange.AddListener(OnHealthChange);

        foreach (var set in spriteSets)
        {
            if (!sprites.ContainsKey(set.maxHealth))
            {
                sprites.Add(set.maxHealth, set.healthSprites);
            }
        }

        OnHealthChange();
    }

    void OnDestroy()
    {
        healthManager.OnHealthChange.RemoveListener(OnHealthChange);
    }

    void OnHealthChange()
    {
        int maxHP = healthManager.GetMaxHealth();
        int currentHP = healthManager.GetCurrentHealth();

        if (!sprites.ContainsKey(maxHP))
        {
            Debug.LogError($"SpriteSet not found for {maxHP} max health value.");

            return;
        }

        if (currentHP < 0 || currentHP >= sprites[maxHP].Length)
        {
            Debug.LogError($"Sprite not found for {currentHP} HP in {maxHP} HP SpriteSet.");

            return;
        }

        image.sprite = sprites[maxHP][currentHP];
    }
}
