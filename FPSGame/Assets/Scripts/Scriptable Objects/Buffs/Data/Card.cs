using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Sprite icon;
    public bool isEquipped;
    public bool isUnlocked;

    public CardType cardType;
    public StatType statType;
    public StatModifier statModifier;

    public float value;
    public StatModificationType type;
    public int order;

    private void Awake()
    {
        if (order == 0)
        {
            statModifier = new StatModifier(value, type, this);
        }

        else
        {
            statModifier = new StatModifier(value, type, order, this);
        }
    }
}