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
    [SerializeField] int magSize;
    [SerializeField] int bulletsPerTap;
    [SerializeField] bool rapidFire;

    private int currentAmmo, bulletsShot;

    // bools
    private bool reloading;

    // Reference
    [SerializeField] Transform cam;
    //public Transform attackPoint;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask enemy;

    WaitForSeconds rapidFireWait;
    WaitForSeconds reloadWait;

    private void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = magSize;
        bulletsShot = bulletsPerTap;
        reloading = false;
    }

    public void Shoot()
    {
        currentAmmo -= bulletsShot;

        if (Physics.Raycast(cam.position, cam.forward, out hit, range))
        {
            if (hit.collider.GetComponent<Damageable>() != null)
            {
                hit.collider.GetComponent<Damageable>().TakeDamage(damage, hit.point, hit.normal);
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

        else
        {
            reloading = true;

            print("reloading");

            yield return reloadWait;
            currentAmmo = magSize;

            print("finished reloading");

            reloading = false;
        }
    }

    private bool CanShoot()
    {
        return currentAmmo > 0 && !reloading;
    }

    public string GetAmmoLeftRatio()
    {
        return new string(currentAmmo + "/" + magSize);
    }
}
