using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum StatType {
    MaxHealth,
    RegenSpeed,
    DefaultMovementSpeed,
    CrouchSpeed,
    SprintSpeed,
    SlideSpeed,
    JumpHeight,
    ReloadSpeedMultiplier,
    DamageMultiplier,
    FireRateMultiplier,
    AmmoMultiplier,
}

[CreateAssetMenu(menuName = "Stats/Player")]
public class StatData : ScriptableObject
{
    public Dictionary<StatType, PlayerStat> stats;

    [Header("Health")]
    public float maxHealth;
    public float regenSpeed;

    [Header("Movement")]
    public float defaultMovementSpeed;
    public float crouchSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float jumpHeight;

    [Header("Base weapon multiplier")]
    public float reloadSpeedMultiplier;
    public float damageMultiplier;
    public float shootSpeedMultiplier;
    public float ammoMultiplier;

    private void OnEnable()
    {
        stats = new Dictionary<StatType, PlayerStat>()
        {
            { StatType.MaxHealth, new PlayerStat(maxHealth) },
            { StatType.RegenSpeed, new PlayerStat(regenSpeed) },
            { StatType.DefaultMovementSpeed, new PlayerStat(defaultMovementSpeed) },
            { StatType.CrouchSpeed, new PlayerStat(crouchSpeed) },
            { StatType.SprintSpeed, new PlayerStat(sprintSpeed) },
            { StatType.SlideSpeed, new PlayerStat(slideSpeed) },
            { StatType.JumpHeight, new PlayerStat(jumpHeight) },
            { StatType.ReloadSpeedMultiplier, new PlayerStat(reloadSpeedMultiplier) },
            { StatType.DamageMultiplier, new PlayerStat(damageMultiplier) },
            { StatType.FireRateMultiplier, new PlayerStat(shootSpeedMultiplier) },
            { StatType.AmmoMultiplier, new PlayerStat(ammoMultiplier) },
        };
    }
}
