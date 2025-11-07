using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1. Creamos una "plantilla" para el inspector.
// [System.Serializable] hace que esta clase aparezca en el Inspector de Unity.
[System.Serializable]
public class HealthSpriteSet
{
    [Tooltip("La vida máxima para este set (ej: 3, 4, 5...)")]
    public int maxHealth; // Para saber qué vida máxima usa (ej: 3)

    [Tooltip("Arrastra los sprites de vida aquí. El índice 0 debe ser 0 HP.")]
    // Aquí arrastrarás los sprites en el orden correcto
    // El índice 0 debe ser el sprite de 0 HP
    // El índice 1 debe ser el sprite de 1 HP
    // etc.
    public Sprite[] healthSprites;
}

[RequireComponent(typeof(Image))]
public class PlayerHeal1 : MonoBehaviour
{
    [SerializeField] private GameObject player;

    // 2. Reemplazamos las texturas por una LISTA de nuestra plantilla
    [Header("Configuración de Sprites de Vida")]
    [SerializeField] private List<HealthSpriteSet> spriteSets;

    private Image image;
    private PlayerHpManager healthManager;

    // El diccionario se sigue usando, pero lo llenaremos desde la lista
    private readonly Dictionary<int, Sprite[]> sprites = new();

    void Start()
    {
        image = GetComponent<Image>();

        // --- Verificación de Errores ---
        if (player == null)
        {
            Debug.LogError("Error en PlayerHeal: No se asignó el objeto 'Player' en el Inspector.");
            return;
        }
        healthManager = player.GetComponent<PlayerHpManager>();
        if (healthManager == null)
        {
            Debug.LogError("Error en PlayerHeal: El objeto 'Player' no tiene el script 'HealthManager'.");
            return;
        }
        // --- Fin Verificación ---

        healthManager.OnHealthChange.AddListener(OnHealthChange);

        // 3. Llenamos el diccionario usando la lista del Inspector
        // Esto es rápido, confiable y a prueba de errores.
        foreach (var set in spriteSets)
        {
            if (!sprites.ContainsKey(set.maxHealth))
            {
                sprites.Add(set.maxHealth, set.healthSprites);
            }
        }

        // 4. ¡Ya no necesitamos AssetDatabase! Todo ese código se va.

        // Llamada inicial para setear la vida completa al empezar
        OnHealthChange();
    }

    void OnDestroy()
    {
        // Buena práctica: desuscribirse si el HealthManager existe
        if (healthManager != null)
        {
            healthManager.OnHealthChange.RemoveListener(OnHealthChange);
        }
    }

    void OnHealthChange()
    {
        int maxHP = healthManager.GetMaxHealth();
        int currentHP = healthManager.GetCurrentHealth();

        // 5. El código de cambio de sprite ahora es más seguro
        if (sprites.ContainsKey(maxHP))
        {
            // Comprobamos que el índice sea válido
            if (currentHP >= 0 && currentHP < sprites[maxHP].Length)
            {
                image.sprite = sprites[maxHP][currentHP];
            }
            else
            {
                Debug.LogWarning($"Error de índice: No hay sprite para {currentHP} HP en el set de {maxHP} HP.");
            }
        }
        else
        {
            Debug.LogWarning($"Error: No se encontró un SpriteSet para {maxHP} de vida máxima.");
        }
    }
}