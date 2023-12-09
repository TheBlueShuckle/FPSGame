using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/RegenBuff")]
public class RegenBuffData : ScriptableBuff
{
    public float hpPerTick;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedRegenBuff(this, obj);
    }
}
