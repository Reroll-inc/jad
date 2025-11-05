using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPowerUp : MonoBehaviour
{
    public CardType powerUpType;

    public TextMeshProUGUI cardNameText;
    public Image cardIcon;

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

    /*public void UpdateCardDisplay(CardType newType)
    {
        switch (powerUpType)
        {
            case CardType.Mage:
                cardNameText.twxt = "El Mago";
                cardIcon.sprite = 
        }
    }*/
}
