using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : CardSlot
{
    [SerializeField] StatData playerStats;
    [SerializeField] TextMeshProUGUI debugText;

    public bool isOccupied;

    private StatType statType;
    private Card lastCard;

    public new Card Card
    {
        get { return card; }
        set 
        { 
            card = value; 
            
            if (card != null && card.tempIsUnlocked)
            {
                isOccupied = true;

                statType = card.statType;
                lastCard = card;

                playerStats.stats[statType].AddModifier(card.StatModifier);

                debugText.text = gameObject.name + ": " + card.statType.ToString() + " " + card.StatModifier.Value;

                image.sprite = card.icon;
            }

            else
            {
                isOccupied = false;

                if (lastCard != null)
                {
                    playerStats.stats[statType].RemoveAllModifiersFromSource(lastCard);
                    print("deleted " + lastCard.name + " from " + gameObject.name);
                }

                debugText.text = gameObject.name + ": none";

                image.sprite = emptySlot;
            }
        }
    }

    protected override void OnValidate()
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

        emptySlot = Resources.Load<Sprite>("images/placeholders/EmptySlot");

        LoadImage();
    }
}
