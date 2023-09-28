using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    // Gun stats
    [SerializeField] int damage;
    [SerializeField] float fireRate;
    [SerializeField] float spread;
    [SerializeField] float range;
    [SerializeField] float reloadTime;
    [SerializeField] float timeBetweenShots;
    [SerializeField] int magSize;
    [SerializeField] int bulletsPerTap;
    [SerializeField] bool rapidFire;

    private int currentAmmo, bulletsShot;

    // bools
    private bool shooting, readyToShoot;

    // Reference
    [SerializeField] Transform cam;
    //public Transform attackPoint;
    [SerializeField] RaycastHit rayHit;
    [SerializeField] LayerMask enemy;

    WaitForSeconds rapidFireWait;
    WaitForSeconds reloadWait;

    private void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = magSize;
    }

    public void Shoot()
    {
        currentAmmo--;

        if (Physics.Raycast(cam.position, cam.forward, out rayHit, range))
        {
            if (rayHit.collider.GetComponent<Damageable>() != null)
            {
                rayHit.collider.GetComponent<Damageable>().TakeDamage(damage);
            }
        }
    }

    public IEnumerator RapidFire()
    {
        if (CanShoot())
        {
            Shoot();

            if (rapidFire)
            {
                while (CanShoot())
                {
                    yield return rapidFireWait;
                    Shoot();
                }

                StartCoroutine(Reload());
            }
        }

        else
        {
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload()
    {
        if (currentAmmo == magSize)
        {
            yield return null;
        }

        print("reloading");

        yield return reloadWait;
        currentAmmo = magSize;

        print("finished reloading");
    }

    private bool CanShoot()
    {
        return currentAmmo > 0;
    }

    public string GetAmmoLeftRatio()
    {
        return new string(currentAmmo + "/" + magSize);
    }
}
