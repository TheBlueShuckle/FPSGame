using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour, IPointerClickHandler
{
    public Card card;

    public event Action<Card> OnLeftClickEvent;

    protected virtual void OnValidate()
    {
        UpdateCard();
    }

    protected void UpdateCard()
    {
        if (card != null && card.isUnlocked)
        {
            GetComponent<Image>().sprite = card.icon;
        }

        else
        {
            GetComponent<Image>().sprite = null;
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData != null && pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (card != null && OnLeftClickEvent != null)
            {
                OnLeftClickEvent(card);
            }
        }
    }
}
