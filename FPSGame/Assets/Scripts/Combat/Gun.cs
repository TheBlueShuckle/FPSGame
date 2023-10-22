using JetBrains.Annotations;
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
    [SerializeField] int bulletSpeed;

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
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask enemy;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject gunbarrel;

    Transform cam;

    WaitForSeconds rapidFireWait;
    WaitForSeconds reloadWait;

    private void Awake()
    {
        cam = GameObject.Find("PlayerCamera").transform;
        cameraShake = cam.GetComponent<CameraShake>();

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

            GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.transform.position, transform.rotation);

            bullet.GetComponent<Bullet>().AssignValues(damageMax, damageMin, damageDropOffStart, damageDropOffEnd, rangeMax);
            bullet.GetComponent<Rigidbody>().velocity = shootDirection * bulletSpeed;
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

    public IEnumerator fire()
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
}
