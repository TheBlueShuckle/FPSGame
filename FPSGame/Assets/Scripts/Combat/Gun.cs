using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] StatData statData;
    [SerializeField] GunData gunData;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask enemy;
    [SerializeField] GameObject muzzleFlashHolder;
    [SerializeField] GameObject muzzleFlash;

    CameraShake cameraShake;

    private int currentAmmo;
    private int damageMax;
    private int damageMin;
    private int magSize;
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

        print(rapidFireWait.Yield());
    }

    private void Update()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        WaitForSeconds updatedWaitForSeconds = new WaitForSeconds(1 / ConvertRPMtoSeconds(gunData.roundsPerMinute * statData.stats[StatType.FireRateMultiplier].Value));

        if (rapidFireWait != updatedWaitForSeconds)
        {
            rapidFireWait = updatedWaitForSeconds;
            print("updated rapidfire");
        }

        WaitForSeconds updatedReloadWait = new WaitForSeconds(1 / (gunData.reloadTime * statData.stats[StatType.ReloadSpeedMultiplier].Value));

        if (reloadWait != updatedReloadWait)
        {
            reloadWait = updatedReloadWait;
        }

        int updatedMaxDamage = (int)(gunData.damageMax * statData.stats[StatType.DamageMultiplier].Value);

        if (damageMax != updatedMaxDamage)
        {
            damageMax = updatedMaxDamage;
            damageMin = (int)(gunData.damageMin * statData.stats[StatType.DamageMultiplier].Value);
        }

        int updatedMagSize = (int)(gunData.magSize * statData.stats[StatType.AmmoMultiplier].Value);

        if (magSize != updatedMagSize)
        {
            magSize = updatedMagSize;
        }
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

    public IEnumerator Fire()
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

        if (currentAmmo == magSize)
        {
            yield break;
        }

        else
        {
            isReloading = true;

            print("reloading");

            yield return reloadWait;
            currentAmmo = magSize;

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

    public void IsMoving(Vector2 input)
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
            return damageMax;
        }

        if (distance >= gunData.damageDropOffEnd)
        {
            return damageMin;
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
