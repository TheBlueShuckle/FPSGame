using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPanel : MonoBehaviour
{
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
    }

    public bool AddCard(Card card)
    {
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (!slot.isOccupied)
            {
                slot.AddCard(card);
                return true;
            }
        }

        return false;
    }

    public bool RemoveCard(Card card)
    {
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot.card == card)
            {
                slot.RemoveCard();
                return true;
            }
        }

        return false;
    }
}
