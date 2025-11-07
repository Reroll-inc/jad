using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPowerUp : MonoBehaviour
{
    private CardType powerUpType;

    [Header("UI References")]
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescriptionText;
    public Image cardIcon;

    [Header("Cards Sprites")]
    public Sprite mageSprite;
    public Sprite chariotSprite;
    public Sprite wheelSprite;
    public Sprite starSprite;

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
