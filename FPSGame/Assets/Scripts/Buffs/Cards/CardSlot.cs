using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected Card card;
    [SerializeField] protected Image image;

    public Card Card
    {
        get { return card; }
        set
        {
            card = value;

            if (card != null && card.tempIsUnlocked)
            {
                image.sprite = card.icon;
                image.enabled = true;
            }

            else
            {
                image.enabled = false;
            }
        }
    }

    public event Action<Card> OnLeftClickEvent;

    protected virtual void OnValidate()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        LoadImage();
    }

    protected void LoadImage()
    {
        if (Card != null && Card.tempIsUnlocked)
        {
            image.sprite = Card.icon;
        }

        else
        {
            image.sprite = null;
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
