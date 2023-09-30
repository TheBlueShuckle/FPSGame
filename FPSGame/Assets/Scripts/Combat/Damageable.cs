using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] public bool isKillable;
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public GameObject hitEffect;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        Instantiate(hitEffect, hitPos, Quaternion.LookRotation(hitNormal));

        if (isKillable)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }


    private void Die()
    {
        print(name + " was destroyed");
        Destroy(gameObject);
    }
}
