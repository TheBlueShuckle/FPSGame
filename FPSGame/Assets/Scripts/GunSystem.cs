using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletsShot;

    // bools
    private bool shooting, readyToShoot, reloading;

    // Reference
    public Transform cam;
    //public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask enemy;

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    public void Shoot()
    {
        if (Physics.Raycast(cam.position, cam.forward, out rayHit, range))
        {
            print(rayHit.collider.name);
        }
    }
}
