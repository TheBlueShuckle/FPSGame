using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Damageable), true)]

public class DamageableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Damageable damageable = (Damageable)target;

        damageable.isKillable = GUILayout.Toggle(damageable.isKillable, "Is killable");

        if (damageable.isKillable)
        {
            damageable.maxHealth = EditorGUILayout.FloatField("Max Health:", damageable.maxHealth);
        }

        damageable.hitEffect = (GameObject)EditorGUILayout.ObjectField("Hit Effect:", damageable.hitEffect, typeof(GameObject), true);
    }
}