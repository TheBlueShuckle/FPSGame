using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 startPosition;

    int damageMax;
    int damageMin;
    float damageDropOffStart;
    float damageDropOffEnd;
    float rangeMax;

    private void Awake()
    {
        startPosition = transform.position;        
    }

    public void AssignValues(int damageMax, int damageMin, float damageDropOffStart, float damageDropOffEnd, float rangeMax)
    {
        this.damageMax = damageMax;
        this.damageMin = damageMin;
        this.damageDropOffStart = damageDropOffStart;
        this.damageDropOffEnd = damageDropOffEnd;
        this.rangeMax = rangeMax;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collided with: " + collision.gameObject.name);
        Transform hitTransform = collision.transform;

        if (hitTransform.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            hitTransform.GetComponent<Damageable>().TakeDamage(GetDamage(), transform.position, transform.position.normalized);
        }

        if (collision.gameObject.tag != "Bullet")
        {
            Destroy(gameObject);
            print("Destroyed bullet");
        }
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > rangeMax)
        {
            print("Distance: " + Vector3.Distance(startPosition, transform.position));

            Destroy(gameObject);
            print("Destroyed bullet");
        }
    }

    private float GetDamage()
    {
        if (Vector3.Distance(startPosition, transform.position) <= damageDropOffStart)
        {
            return damageMax;
        }

        if (Vector3.Distance(startPosition, transform.position) >= damageDropOffEnd)
        {
            return damageMin;
        }

        float dropOffRange = damageDropOffEnd - damageDropOffStart;
        float distanceNormalised = (Vector3.Distance(startPosition, transform.position) - damageDropOffStart) / dropOffRange;

        return Mathf.RoundToInt(Mathf.Lerp(damageMax, damageMin, distanceNormalised));
    }
}
