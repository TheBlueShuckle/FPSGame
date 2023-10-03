using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun stats")]
    [SerializeField] float fireRate;
    [SerializeField] float spread;
    [SerializeField] float reloadTime;
    [SerializeField] int magSize;
    [SerializeField] int bulletsPerTap;
    [SerializeField] bool rapidFire;

    [Header("Damage")]
    [SerializeField] int damageMax;
    [SerializeField] int damageMin;
    [SerializeField] float damageDropOffStart;
    [SerializeField] float damageDropOffEnd;
    [SerializeField] float rangeMax;

    [Header("Camera Shake")]
    [SerializeField] float secondsDuration;
    [SerializeField] float magnitude;
    CameraShake cameraShake;

    private int currentAmmo;
    private float tempSpread;
    private Vector3 shootDirection;

    // bools
    private bool reloading;
    private bool isShooting = false; 
    private bool isRunning = false;

    [Header("References")]
    Transform cam;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask enemy;
    [SerializeField] GameObject muzzleFlash;
    PickUpController pickUpController;

    WaitForSeconds rapidFireWait;
    WaitForSeconds reloadWait;

    private void Awake()
    {
        pickUpController = GetComponent<PickUpController>();

        if (pickUpController.equipped)
        {
            SetPlayerVars();
        }

        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        muzzleFlash.SetActive(false);
        currentAmmo = magSize;
        reloading = false;
    }

    private void Update()
    {
        if (cam == null || cameraShake == null)
        {
            SetPlayerVars();
        }

        if (!isShooting)
        {
            muzzleFlash.SetActive(false);
        }
    }

    private void SetPlayerVars()
    {
        cam = gameObject.GetComponentInParent<Camera>().transform;
        cameraShake = gameObject.GetComponentInParent<CameraShake>();
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

    public void OnDrop()
    {
        StopAllCoroutines();
        IsShooting(false);
        muzzleFlash.SetActive(false);
    }

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
