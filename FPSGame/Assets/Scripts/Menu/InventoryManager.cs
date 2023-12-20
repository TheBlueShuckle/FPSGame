using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] CardPanel cardPanel;

    private void Awake()
    {
        inventory.OnCardLeftClickedEvent += Equip;
        cardPanel.OnCardLeftClickedEvent += Unequip;
    }

    public void Equip(Card card)
    {
        print("Equipped");

        if (card.isUnlocked)
        {
            cardPanel.AddCard(card);
        }
    }

    public void Unequip(Card card)
    {
        cardPanel.RemoveCard(card);
    }
}
