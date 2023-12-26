using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDescription : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] TextMeshProUGUI cardNameText;
    [SerializeField] TextMeshProUGUI cardTypeText;
    [SerializeField] TextMeshProUGUI cardDescriptionText;

    public void ShowDescription(Card card)
    {
        image.sprite = card.icon;
        cardNameText.text = card.name;
        cardTypeText.text = card.cardType.ToString();
        cardDescriptionText.text = card.description;

        gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        gameObject.SetActive(false);
    }
}
