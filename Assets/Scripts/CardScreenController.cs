using UnityEngine;
using System.Collections.Generic; //List use
using System.Linq; //Cast() and .OrderBy()
using TMPro;

public class CardScreenController : MonoBehaviour
{
    public GameObject cardScreenPanel;

    public CardPowerUp cardOption1;
    public CardPowerUp cardOption2;

    public TextMeshProUGUI titleText;

    public void ShowCardSelection()
    {
        Time.timeScale = 0f;
        cardScreenPanel.SetActive(true);

        List<CardType> allCards = System.Enum.GetValues(typeof(CardType))
            .Cast<CardType>()
            .ToList();

        System.Random rng = new System.Random();
        List<CardType> shuffledCards = allCards.OrderBy(c => rng.Next()).ToList();

        if (shuffledCards.Count >= 2)
        {
            cardOption1.gameObject.SetActive(true);
            cardOption1.SetupCard(shuffledCards[0]);

            cardOption2.gameObject.SetActive(true);
            cardOption2.SetupCard(shuffledCards[1]);
        }  
    }

    public void HideCardSelection()
    {
        cardScreenPanel.SetActive(false);
    }
}