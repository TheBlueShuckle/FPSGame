using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Card> unlockedCards;
    [SerializeField] Transform itemsParent;
    [SerializeField] CardSlot[] cardSlots;

    public event Action<Card> OnCardLeftClickedEvent;

    private void Start()
    {
        RefreshUI();

        unlockedCards = new List<Card>();

        foreach (CardSlot cardSlot in cardSlots)
        {
            cardSlot.OnLeftClickEvent += OnCardLeftClickedEvent;
        }
    }

    private void OnValidate()
    {
        if (itemsParent != null)
        {
            cardSlots = itemsParent.GetComponentsInChildren<CardSlot>();
        }
    }

    private void RefreshUI()
    {
        foreach (CardSlot cardSlot in cardSlots)
        {
            if (cardSlot.card != null && cardSlot.card.isUnlocked)
            {
                unlockedCards.Add(cardSlot.card);
            }
        }
    }
}
