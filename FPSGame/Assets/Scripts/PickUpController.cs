using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] public Gun gun;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public BoxCollider coll;
    [SerializeField] public Transform player;
    [SerializeField] public Transform gunContainer;
    [SerializeField] public Transform cam;

    [SerializeField] public float pickUpRange;
    [SerializeField] public float dropForwardForce;
    [SerializeField] public float dropUpwardForce;

    [SerializeField] public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        if (!equipped)
        {
            gun.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
            gameObject.layer = 7;
        }

        else
        {
            gun.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    public void PickUp()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && !slotFull)
        {
            equipped = true;
            slotFull = true;

            transform.SetParent(gunContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;

            rb.isKinematic = true;
            coll.isTrigger = true;

            gun.enabled = true;

            gameObject.layer = 0;
        }
    }

    public void Drop()
    {
        if (equipped)
        {
            gun.StopAllCoroutines();
            gun.IsShooting(false);

            equipped = false;
            slotFull = false;

            transform.SetParent(null);

            rb.isKinematic = false;
            coll.isTrigger = false;

            rb.velocity = player.GetComponent<CharacterController>().velocity;

            rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
            rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

            float random = Random.Range(-1f, 1f);
            rb.AddTorque(new Vector3(random, random, random) * 10);

            gun.enabled = false;

            gameObject.layer = 7;
        }
    }
}
