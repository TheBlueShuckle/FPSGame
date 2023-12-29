using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;

[Serializable]
public class PlayerStat
{
    public float baseValue;

    public virtual float Value
    {
        get
        {
            if (valueIsOutdated || baseValue != lastBaseValue)
            {
                lastBaseValue = baseValue;
                updatedValue = CalculateFinalValue();
                valueIsOutdated = false;
            }
            return updatedValue;
        }
    }

    protected bool valueIsOutdated = true;
    protected float updatedValue;
    protected float lastBaseValue = float.MinValue;

    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public PlayerStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public PlayerStat(float baseValue) : this()
    {
        this.baseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier modifier)
    {
        valueIsOutdated = true;
        statModifiers.Add(modifier);
        statModifiers.Sort(CompareModifierOrder);

        if (modifier is TimedStatModifier timedModifier)
        {
            timedModifier.StartTimer();
        }
    }

    public void UpdateTimedModifiers(float deltaTime)
    {
        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i] is TimedStatModifier timedModifier)
            {
                timedModifier.Update(deltaTime);

                if (timedModifier.CheckIfDone())
                {
                    statModifiers.Remove(statModifiers[i]);
                }
            }
        }
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order > b.Order)
        {
            return 1;
        }

        if (a.Order < b.Order)
        {
            return -1;
        }

        return 0;
    }

    public virtual bool RemoveModifier(StatModifier modifier)
    {
        if (statModifiers.Remove(modifier))
        {
            valueIsOutdated = true;
            return true;
        }

        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool removedModifier = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                valueIsOutdated = true;
                removedModifier = true;
                statModifiers.RemoveAt(i);
            }
        }

        return removedModifier;
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float sumPercentAdd = 0;

        foreach (StatModifier statModifier in statModifiers)
        {
            if (statModifier.Type == StatModificationType.Flat)
            {
                finalValue += statModifier.Value;
            }

            else if (statModifier.Type == StatModificationType.PercentAdd)
            {
                sumPercentAdd += statModifier.Value;

                //If the modifier is the last one or is the last of the same type
                if (statModifiers.IndexOf(statModifier) + 1 >= statModifiers.Count || statModifiers[statModifiers.IndexOf(statModifier) + 1].Type != StatModificationType.PercentAdd)
                {
                    finalValue *= 1 + (sumPercentAdd / 100);
                }
            }

            else if (statModifier.Type == StatModificationType.PercentMultiply)
            {
                finalValue *= 1 + (statModifier.Value / 100);
            }
        }

        return (float)Math.Round(finalValue, 4);
    }
}
