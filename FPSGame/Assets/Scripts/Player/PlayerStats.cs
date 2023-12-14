using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] BaseStatData playerBaseStats;

    float maxHealth;
    float regenSpeed;

    float defaultMovementSpeed;
    float crouchSpeed;
    float sprintSpeed;
    float slideSpeed;
    float jumpHeight;

    private void Awake()
    {
        maxHealth = playerBaseStats.maxHealth;
        regenSpeed = playerBaseStats.regenSpeed;

        defaultMovementSpeed = playerBaseStats.defaultMovementSpeed;
        crouchSpeed = playerBaseStats.crouchSpeed;
        sprintSpeed = playerBaseStats.sprintSpeed;
        slideSpeed = playerBaseStats.slideSpeed;
        jumpHeight = playerBaseStats.jumpHeight;
    }
}
