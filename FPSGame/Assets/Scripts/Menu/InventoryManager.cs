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
        if (card.tempIsUnlocked && !card.tempIsEquipped)
        {
            cardPanel.AddCard(card);
            print("Equipped " + card.name);
        }
    }

    public void Unequip(Card card)
    {
        print("Unequipped " + card.name);

        cardPanel.RemoveCard(card);
    }
}
