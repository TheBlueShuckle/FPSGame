using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    static int Interactable = 7, Weapons = 9;

    [SerializeField] public Gun gun;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public BoxCollider coll;
    Transform player;
    Transform gunContainer;
    Transform cam;

    [SerializeField] public float pickUpRange;
    [SerializeField] public float dropForwardForce;
    [SerializeField] public float dropUpwardForce;

    [SerializeField] public bool equipped;
    public static bool slotFull;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        cam = GameObject.Find("PlayerCamera").transform;
        gunContainer = GameObject.Find("GunContainer").transform;
    }

    private void Start()
    {
        if (!equipped)
        {
            gun.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;

            SetLayerAllChildren(transform, Interactable);
        }

        else
        {
            gun.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;

            SetLayerAllChildren(transform, Weapons);
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

            SetLayerAllChildren(transform, Weapons);
        }
    }

    public void Drop()
    {
        if (equipped)
        {
            gun.OnDrop();

            equipped = false;
            slotFull = false;

            transform.SetParent(null);

            rb.isKinematic = false;
            coll.isTrigger = false;

            ThrowGun();

            gun.enabled = false;

            SetLayerAllChildren(transform, Interactable);
        }
    }

    private void SetGameLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;

        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.layer = layer;

            Transform hasChildren = child.GetComponent<Transform>();
            if (hasChildren != null)
            {
                SetGameLayerRecursive(child.gameObject, layer);
            }
        }
    }

    private void SetLayerAllChildren(Transform root, int layer)
    {
        Transform[] children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (Transform child in children)
        {
            child.gameObject.layer = layer;
        }
    }

    private void ThrowGun()
    {
        rb.velocity = player.GetComponent<CharacterController>().velocity;

        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }
}
