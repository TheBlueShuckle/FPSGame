using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : CardSlot
{
    public bool isOccupied;

    protected override void OnValidate()
    {
        if (isOccupied)
        {
            base.OnValidate();
        }
    }

    public void AddCard(Card card)
    {
        this.card = card;
        isOccupied = true;
        card.isEquipped = true;
        UpdateCard();
    }

    public void RemoveCard()
    {
        card.isEquipped = false;
        card = null;
        isOccupied = false;
        UpdateCard();
    }
}
