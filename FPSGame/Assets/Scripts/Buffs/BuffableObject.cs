using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffableObject : MonoBehaviour
{
    private readonly Dictionary<ScriptableBuff, TimedBuff> buffs = new Dictionary<ScriptableBuff, TimedBuff>();

    void Update()
    {
        //ADD, return before updating each buff if game is paused
        //if (Game.isPaused)
        //    return;

        foreach (TimedBuff buff in buffs.Values.ToList())
        {
            buff.Tick(Time.deltaTime);
            if (buff.isFinished)
            {
                buffs.Remove(buff.buff);
            }
        }
    }

    public void AddBuff(TimedBuff buff)
    {
        if (buffs.ContainsKey(buff.buff))
        {
            buffs[buff.buff].Activate();
        }
        else
        {
            buffs.Add(buff.buff, buff);
            buff.Activate();
        }
    }
}