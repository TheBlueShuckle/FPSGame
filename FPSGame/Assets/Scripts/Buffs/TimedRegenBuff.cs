using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedRegenBuff : TimedBuff
{
    private readonly PlayerHealth playerHealth;
    private RegenBuffData regenBuff;

    public TimedRegenBuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    protected override void ApplyEffect()
    {
        regenBuff = (RegenBuffData)buff;
    }
    protected override void ApplyTick()
    {
        playerHealth.RestoreHealth(regenBuff.hpPerTick);
    }

    public override void End()
    {
        // Do nothing
    }
}
