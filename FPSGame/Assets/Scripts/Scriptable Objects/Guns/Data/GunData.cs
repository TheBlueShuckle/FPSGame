using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Gun stats")]
    public float roundsPerMinute;
    public float spread;
    public float reloadTime;
    public int magSize;
    public int bulletsPerTap;
    public bool rapidFire;

    [Header("Damage")]
    public int damageMax;
    public int damageMin;
    public float damageDropOffStart;
    public float damageDropOffEnd;
    public float rangeMax;

    [Header("Camera Shake")]
    public float shakeDurationSeconds;
    public float shakeMagnitude;
}