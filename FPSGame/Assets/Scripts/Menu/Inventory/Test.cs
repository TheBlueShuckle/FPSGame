using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] StatData playerStats;
    [SerializeField] TextMeshProUGUI text;

    private void Update()
    {
        string debugText = "";

        foreach (PlayerStat stat in playerStats.stats.Values)
        {
            if (stat.StatModifiers.Count > 0)
            {
                string statTypeString = playerStats.stats.FirstOrDefault(x => x.Value == stat).Key.ToString();

                debugText += $"{statTypeString}: \nCount: {stat.StatModifiers.Count} \n" ;

                foreach (StatModifier modifier in stat.StatModifiers)
                {
                    debugText += $"{modifier.Value}, ";

                    if (stat.StatModifiers.IndexOf(modifier) >= stat.StatModifiers.Count -1)
                    {
                        debugText += "\n\n";
                    }
                }
            }
        }

        text.text = debugText;
    }
}
