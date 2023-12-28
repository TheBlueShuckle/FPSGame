using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : CardSlot
{
    [SerializeField] TextMeshProUGUI debugText;

    public bool isOccupied;

    public new Card Card
    {
        get { return card; }
        set 
        { 
            card = value; 
            
            if (card != null && card.tempIsUnlocked)
            {
                isOccupied = true;

                debugText.text = gameObject.name + ": " + card.statType.ToString() + " " + card.StatModifier.Value;

                image.sprite = card.icon;
            }

            else
            {
                isOccupied = false;

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
