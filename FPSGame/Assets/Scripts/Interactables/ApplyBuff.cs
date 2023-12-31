using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuff : Interactable
{
    [SerializeField] private SpeedBuffData speedBuffData;
    [SerializeField] private RegenBuffData regenBuffData;
    private GameObject player;
    private BuffableObject obj;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void OnLookStart()
    {
        base.OnLookStart();
    }

    protected override void Interact()
    {
        obj = player.gameObject.GetComponent<BuffableObject>();

        obj.AddBuff(new TimedSpeedBuff(speedBuffData, player));
        obj.AddBuff(new TimedRegenBuff(regenBuffData, player));
        print("Added buff to " + player.name);

        gameObject.SetActive(false);
    }

    public override void OnLookStop()
    {
        base.OnLookStop();
    }
}
