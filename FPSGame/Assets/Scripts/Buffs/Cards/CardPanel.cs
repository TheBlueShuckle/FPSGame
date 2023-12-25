using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardPanel : MonoBehaviour
{
    [SerializeField] List<Card> equippedCards;
    [SerializeField] Transform cardSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;

    public event Action<Card> OnCardLeftClickedEvent;

    private void Start()
    {
        foreach (EquipmentSlot equipmentSlot in equipmentSlots)
        {
            equipmentSlot.OnLeftClickEvent += OnCardLeftClickedEvent;
        }
    }

    private void OnValidate()
    {
        equipmentSlots = cardSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        UpdateCards();
    }

    public bool AddCard(Card card)
    {
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (!slot.isOccupied && !card.tempIsEquipped)
            {
                equippedCards.Add(card);
                card.tempIsEquipped = true;
                UpdateCards();

                return true;
            }
        }

        UpdateCards();

        return false;
    }

    public bool RemoveCard(Card card)
    {
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot.Card == card)
            {
                equippedCards.Remove(card);
                card.tempIsEquipped = false;
                UpdateCards();

                return true;
            }
        }

        return false;
    }

    private void UpdateCards()
    {
        int i = 0;
        for (; i < equippedCards.Count && i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].Card = equippedCards[i];
        }

        for (; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].Card = null;
        }
    }
}
