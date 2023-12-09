using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpeedBuff : TimedBuff
{
    private readonly PlayerMotor playerMotor;

    public TimedSpeedBuff(ScriptableBuff buff, GameObject obj) : base(buff, obj)
    {
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
    }

    protected override void ApplyEffect()
    {
        SpeedBuffData speedBuff = (SpeedBuffData)buff;
        playerMotor.CurrentSpeedBuff = speedBuff.speedIncrease;
        Debug.Log("Current buff: " + speedBuff.speedIncrease);
    }

    public override void End()
    {
        SpeedBuffData speedBuff = (SpeedBuffData)buff;
        playerMotor.CurrentSpeedBuff = 0.0f;
    }
    protected override void ApplyTick()
    {
        //Do nothing
    }
}
