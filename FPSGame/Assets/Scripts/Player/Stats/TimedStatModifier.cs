using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimedStatModifier : StatModifier
{
    public readonly float TickRate;
    public readonly float Duration;
    Timer timer;

    public TimedStatModifier(float value, StatModificationType type, float duration, int order, object source, float tickRate = 0) : base(value, type, order, source)
    {
        Duration = duration;
        TickRate = tickRate;
    }

    // Default order (depending on type of modifier), no source
    public TimedStatModifier(float value, StatModificationType type, float duration, float tickRate = 0) : this(value, type, duration, (int)type, null, tickRate) { }
    // Custom order, no source
    public TimedStatModifier(float value, StatModificationType type, float duration, int order, float tickRate = 0) : this(value, type, duration, order, null, tickRate) { }
    // Default order, with source
    public TimedStatModifier(float value, StatModificationType type, float duration, object source, float tickRate = 0) : this(value, type, duration, (int)type, source, tickRate) { }

    public void StartTimer()
    {
        timer = new Timer(Duration);
    }

    public void Update(float deltaTime)
    {
        timer.Update(deltaTime);
    }

    public bool CheckIfDone()
    {
        if (timer.IsDone())
        {
            return true;
        }

        return false;
    }
}