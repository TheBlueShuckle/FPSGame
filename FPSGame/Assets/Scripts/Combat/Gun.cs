using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GunData gunData;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask enemy;
    [SerializeField] GameObject muzzleFlashHolder;
    [SerializeField] GameObject muzzleFlash;

    CameraShake cameraShake;

    private int currentAmmo;
    private float tempSpread;
    private Vector3 shootDirection;

    private bool isReloading = false;
    private bool isRunning = false;

    Transform cam;

    WaitForSeconds rapidFireWait;
    WaitForSeconds reloadWait;

    private void Awake()
    {
        cam = GameObject.Find("PlayerCamera").transform;
        cameraShake = cam.GetComponent<CameraShake>();

        rapidFireWait = new WaitForSeconds(1 / ConvertRPMtoSeconds(gunData.roundsPerMinute));
        reloadWait = new WaitForSeconds(gunData.reloadTime);
        currentAmmo = gunData.magSize;
    }

    public void Shoot()
    {
        currentAmmo--;
        StartCoroutine(cameraShake.Shake(gunData.shakeDurationSeconds, gunData.shakeMagnitude));

        GameObject gm = Instantiate(muzzleFlash, muzzleFlashHolder.transform.position, cam.rotation);
        gm.transform.parent = muzzleFlashHolder.transform;

        for (int i = 0; i < gunData.bulletsPerTap; i++)
        {
            AddSpread();

            if (Physics.Raycast(cam.position, shootDirection, out hit, gunData.rangeMax))
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
            tempSpread = gunData.spread * 2;
        }

        else
        {
            tempSpread = gunData.spread;
        }

        float x = Random.Range(-tempSpread, tempSpread);
        float y = Random.Range(-tempSpread, tempSpread);

        shootDirection = cam.transform.forward + new Vector3(x, y, 0);
    }

    public IEnumerator fire()
    {
        if (CanShoot())
        {
            Shoot();

            if (gunData.rapidFire)
            {
                while (CanShoot())
                {
                    yield return rapidFireWait;
                    Shoot();
                }
            }

            if (currentAmmo <= 0)
            {
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
        if (isReloading)
        {
            yield break;
        }

        if (currentAmmo == gunData.magSize)
        {
            yield break;
        }

        else
        {
            isReloading = true;

            print("reloading");

            yield return reloadWait;
            currentAmmo = gunData.magSize;

            print("finished reloading");

            isReloading = false;
        }
    }

    private bool CanShoot()
    {
        return currentAmmo > 0 && !isReloading;
    }

    public string GetAmmoLeftRatio()
    {
        return new string(currentAmmo + "/" + gunData.magSize);
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

    public void OnDrop()
    {
        StopAllCoroutines();
        isReloading = false;
    }

    private int GetDamageDropoff(float distance)
    {
        if (distance <= gunData.damageDropOffStart)
        {
            return gunData.damageMax;
        }

        if (distance >= gunData.damageDropOffEnd)
        {
            return gunData.damageMin;
        }

        float dropOffRange = gunData.damageDropOffEnd - gunData.damageDropOffStart;

        float distanceNormalised = (distance - gunData.damageDropOffStart) / dropOffRange;

        return Mathf.RoundToInt(Mathf.Lerp(gunData.damageMax, gunData.damageMin, distanceNormalised));
    }

    private float ConvertRPMtoSeconds(float RPM)
    {
        return RPM / 60;
    }
}
