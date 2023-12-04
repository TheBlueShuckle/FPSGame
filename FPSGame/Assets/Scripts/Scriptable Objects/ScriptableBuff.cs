using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableBuff : ScriptableObject
{
    public float SecondsDuration;

    public bool IsDurationStacked;

    public bool IsEffectStacked;

    public abstract TimedBuff InitializeBuff(GameObject obj);
}
