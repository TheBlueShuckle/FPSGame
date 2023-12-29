
public enum StatModificationType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMultiply = 300,
}

public class StatModifier
{
    public readonly float Value;
    public readonly StatModificationType Type;
    public readonly int Order;
    public readonly object Source;

    // Custom order
    public StatModifier(float value, StatModificationType type, int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    // Default order (depending on type of modifier), no source
    public StatModifier(float value, StatModificationType type) : this (value, type, (int)type, null) { }
    // Custom order, no source
    public StatModifier(float value, StatModificationType type, int order) : this(value, type, order, null) { }
    // Default order, with source
    public StatModifier(float value, StatModificationType type, object source) : this(value, type, (int)type, source) { }
}