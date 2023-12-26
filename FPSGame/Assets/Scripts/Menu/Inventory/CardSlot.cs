using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Card card;
    [SerializeField] protected Image image;
    [SerializeField] protected CardDescription description;

    protected Sprite emptySlot;

    public Card Card
    {
        get { return card; }
        set
        {
            card = value;

            if (card != null && card.tempIsUnlocked)
            {
                image.sprite = card.icon;
            }

            else
            {
                image.sprite = emptySlot;
            }
        }
    }

    public event Action<Card> OnLeftClickEvent;

    protected virtual void OnValidate()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            image.enabled = true;
        }

        if (description == null)
        {
            description = FindObjectOfType<CardDescription>();
        }

        emptySlot = Resources.Load<Sprite>("images/placeholders/LockedSlot");

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
            image.sprite = emptySlot;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (card != null)
        {
            description.ShowDescription(card);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.HideDescription();
    }
}
