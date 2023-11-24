using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombiesPerRound", menuName = "Rounds")]
public class RoundData : ScriptableObject
{
    public int[] enemiesPerRound;
}
