using UnityEngine;

public abstract class TimedBuff
{
    protected float tickRateSeconds = 0.5f;
    protected float duration;
    protected int effectStacks;
    protected readonly GameObject obj;
    public bool isFinished;
    private float timeSinceLastTick;
    public ScriptableBuff buff { get; }

    public TimedBuff(ScriptableBuff buff, GameObject obj)
    {
        this.buff = buff;
        this.obj = obj;
    }

    public void Tick(float delta)
    {
        duration -= delta;
        timeSinceLastTick += delta;

        if (timeSinceLastTick >= tickRateSeconds)
        {
            ApplyTick();
            timeSinceLastTick = 0;
        }
        if (duration <= 0)
        {
            End();
            isFinished = true;
        }
    }

    public void Activate()
    {
        if (buff.IsEffectStacked || duration <= 0)
        {
            ApplyEffect();
            effectStacks++;
        }

        if (buff.IsDurationStacked || duration <= 0)
        {
            duration += buff.SecondsDuration;
        }
    }
    protected abstract void ApplyEffect();

    protected abstract void ApplyTick();

    public abstract void End();
}