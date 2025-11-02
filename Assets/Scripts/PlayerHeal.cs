using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerHeal : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Texture2D lifeBar3;
    [SerializeField] private Texture2D lifeBar4;
    [SerializeField] private Texture2D lifeBar5;
    [SerializeField] private Texture2D lifeBar6;
    [SerializeField] private Texture2D lifeBar7;

    private Image image;
    private HealthManager healthManager;
    private readonly Dictionary<int, Sprite[]> sprites = new();

    void Start()
    {
        image = GetComponent<Image>();
        healthManager = player.GetComponent<HealthManager>();

        healthManager.OnHealthChange.AddListener(OnHealthChange);

        sprites.Add(3, AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(lifeBar3)).OfType<Sprite>().ToArray());
        sprites.Add(4, AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(lifeBar4)).OfType<Sprite>().ToArray());
        sprites.Add(5, AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(lifeBar5)).OfType<Sprite>().ToArray());
        sprites.Add(6, AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(lifeBar6)).OfType<Sprite>().ToArray());
        sprites.Add(7, AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(lifeBar7)).OfType<Sprite>().ToArray());
    }

    void OnDestroy()
    {
        healthManager.OnHealthChange.RemoveListener(OnHealthChange);
    }

    void OnHealthChange()
    {
        image.sprite = sprites[healthManager.GetMaxHealth()][healthManager.GetCurrentHealth() ];
    }
}
