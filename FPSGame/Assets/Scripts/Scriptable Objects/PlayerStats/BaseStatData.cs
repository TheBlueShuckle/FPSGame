using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player")]
public class BaseStatData : ScriptableObject
{
    [Header("Health")]
    public float maxHealth;
    public float regenSpeed;

    [Header("Movement")]
    public float defaultMovementSpeed;
    public float crouchSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float jumpHeight;
}
