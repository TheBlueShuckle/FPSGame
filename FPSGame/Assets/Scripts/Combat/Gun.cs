using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun stats")]
    [SerializeField] int damageMax;
    [SerializeField] int damageMin;
    [SerializeField] float fireRate;
    [SerializeField] float spread;
    [SerializeField] float rangeMax;
    [SerializeField] float reloadTime;
    [SerializeField] int magSize;
    [SerializeField] int bulletsPerTap;
    [SerializeField] bool rapidFire;
    [SerializeField] float damageDropOffStart;
    [SerializeField] float damageDropOffEnd;

    [Header("Camera Shake")]
    [SerializeField] float secondsDuration;
    [SerializeField] float magnitude;
    [SerializeField] CameraShake cameraShake;

    private int currentAmmo;
    private float tempSpread;
    private Vector3 shootDirection;

    // bools
    private bool reloading;
    private bool isShooting = false; 
    private bool isRunning = false;

    [Header("References")]
    [SerializeField] Transform cam;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask enemy;
    [SerializeField] GameObject muzzleFlash;

    WaitForSeconds rapidFireWait;
    WaitForSeconds reloadWait;

    private void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        muzzleFlash.SetActive(false);
        currentAmmo = magSize;
        reloading = false;
    }

    private void Update()
    {
        if (!isShooting)
        {
            muzzleFlash.SetActive(false);
        }
    }

    public void Shoot()
    {
        currentAmmo--;
        StartCoroutine(cameraShake.Shake(secondsDuration, magnitude));

        for (int i = 0; i < bulletsPerTap; i++)
        {
            AddSpread();
            muzzleFlash.SetActive(true);

            if (Physics.Raycast(cam.position, shootDirection, out hit, rangeMax))
            {
                if (hit.collider.GetComponent<Damageable>() != null)
                {
                    int calcDamage;
                    hit.collider.GetComponent<Damageable>().TakeDamage(calcDamage = GetDamageDropoff(hit.distance), hit.point, hit.normal);
                    print($"Dealt {calcDamage} damage");
                }
            }
        }
    }

    private void AddSpread()
    {
        if (isRunning)
        {
            tempSpread = spread * 2;
        }

        else
        {
            tempSpread = spread;
        }

        float x = Random.Range(-tempSpread, tempSpread);
        float y = Random.Range(-tempSpread, tempSpread);

        shootDirection = cam.transform.forward + new Vector3(x, y, 0);
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
            isShooting = false;

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

    public void isMoving(Vector2 input)
    {
        if(input.x != 0 || input.y != 0)
        {
            isRunning = true;
        }

        else
        {
            isRunning = false;
        }
    }

    public void IsShooting(bool isShooting)
    {
        this.isShooting = isShooting;
    }

    // complicated damageDropoff
    /*
    private int GetDramageDropoff(float distance, float maxDistance)
    {
        return Mathf.RoundToInt(damage * (-0.0000454f * Mathf.Exp(10 * (distance / maxDistance)) + 1));
    }
    */

    private int GetDamageDropoff(float distance)
    {
        if (distance <= damageDropOffStart)
        {
            return damageMax;
        }

        if (distance >= damageDropOffEnd)
        {
            return damageMin;
        }

        float dropOffRange = damageDropOffEnd - damageDropOffStart;

        float distanceNormalised = (distance - damageDropOffStart) / dropOffRange;

        return Mathf.RoundToInt(Mathf.Lerp(damageMax, damageMin, distanceNormalised));
    }
}
