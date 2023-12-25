using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : CardSlot
{
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

                image.sprite = card.icon;
                image.enabled = true;
            }

            else
            {
                isOccupied = false;

                image.enabled = false;
            }
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }
}
