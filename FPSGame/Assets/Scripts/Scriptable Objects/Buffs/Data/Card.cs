using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum CardType
{
    Hearts,
    Diamonds,
    Spades,
    Clubs,
}

[CreateAssetMenu(menuName = "Buffs/Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite icon;
    [SerializeField] private bool isEquipped;
    [SerializeField] private bool isUnlocked;

    public CardType cardType;
    public StatType statType;
    private StatModifier statModifier;

    public StatModifier StatModifier
    {
        get 
        { 
            if (statModifier == null)
            {
                if (order == 0)
                {
                    StatModifier = new StatModifier(value, type, this);
                }

                else
                {
                    StatModifier = new StatModifier(value, type, order, this);
                }
            }

            return statModifier; 
        }
        set { statModifier = value; }
    }

    public float value;
    public StatModificationType type;
    public int order;

    [NonSerialized] public bool tempIsEquipped;
    [NonSerialized] public bool tempIsUnlocked;

    private void OnValidate()
    {
        Debug.Log(tempIsEquipped = isEquipped);
        Debug.Log(tempIsUnlocked = isUnlocked);
    }
}