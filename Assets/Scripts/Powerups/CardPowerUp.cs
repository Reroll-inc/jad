using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CardType
{
    Mage,
    Chariot,
    Wheel,
    Star,
}

[RequireComponent(typeof(Button))]
public class CardPowerUp : MonoBehaviour
{
    private CardType powerUpType;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    [SerializeField] private Image cardIcon;

    [Header("Cards Sprites")]
    [SerializeField] private Sprite mageSprite;
    [SerializeField] private Sprite chariotSprite;
    [SerializeField] private Sprite wheelSprite;
    [SerializeField] private Sprite starSprite;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectCard);
    }

    void SelectCard()
    {
        LevelManager.Instance.CardSelect(powerUpType);
    }

    public void SetupCard(CardType newType)
    {
        powerUpType = newType;

        switch (powerUpType)
        {
            case CardType.Mage:
                cardNameText.text = "El Mago";
                cardDescriptionText.text = "+10% Velocidad de Ataque";
                cardIcon.sprite = mageSprite;
                break;
            case CardType.Chariot:
                cardNameText.text = "El Carro";
                cardDescriptionText.text = "+10% Velocidad de Movimiento";
                cardIcon.sprite = chariotSprite;
                break;
            case CardType.Wheel:
                cardNameText.text = "La Rueda de la Fortuna";
                cardDescriptionText.text = "-10% Enfriamiento de Dash";
                cardIcon.sprite = wheelSprite;
                break;
            case CardType.Star:
                cardNameText.text = "La Estrella";
                cardDescriptionText.text = "+10% Tamaño de Proyectil";
                cardIcon.sprite = starSprite;
                break;
        }
    }
}
