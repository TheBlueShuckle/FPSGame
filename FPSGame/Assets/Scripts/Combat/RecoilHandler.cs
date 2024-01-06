using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RecoilHandler : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private RecoilProperties properties;
    private Gun gun;

    [SerializeField] Camera cam;

    private void Awake()
    {
        SetProperties();
        gun = properties.transform.GetComponent<Gun>();
    }

    void Update()
    {
        if (properties != null)
        {
            float returnSpeed = properties.returnSpeed;
            float snappiness = properties.snappiness;

            if (!gun.IsShooting)
            {
                targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
            }

            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

            transform.localRotation = Quaternion.Euler(currentRotation);
        }

        if (properties == null || !properties.transform.IsChildOf(transform))
        {
            SetProperties();
        }

        if(properties != null && (gun == null || !gun.transform.IsChildOf(transform)))
        {
            gun = properties.transform.GetComponent<Gun>();
        }
    }

    private float GetAjdustedAngles(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }

        return angle;
    }

    private float burrh(float value)
    {
        if (value > 0)
        {
            return -90 + value;
        }

        return -90 - value;
    }

    private void SetProperties()
    {
        properties = null;

        foreach (GameObject weapon in GameObject.FindGameObjectsWithTag("Weapon"))
        {
            if (weapon.transform.IsChildOf(transform))
            {
                properties = weapon.GetComponent<RecoilProperties>();
                print("Got the properties from " + properties.transform.name);
                break;
            }
        }
    }

    public void Recoil()
    {
        float recoilX = properties.recoilX;
        float recoilY = Random.Range(-properties.recoilY, properties.recoilY);
        float recoilZ = Random.Range(-properties.recoilZ, properties.recoilZ);

        targetRotation += new Vector3(recoilX, recoilY, recoilZ);

        float targetXAngle = GetAjdustedAngles(Quaternion.Euler(targetRotation).eulerAngles.x);
        float cameraXAngle = GetAjdustedAngles(cam.transform.localRotation.eulerAngles.x);

        //print($"Transform angle: {targetXAngle}, camera angle: {cameraXAngle}");

        if (cameraXAngle + targetXAngle < -90)
        {
            targetRotation.x = -90 - cameraXAngle;
            print("set targetRotation.x to " + targetRotation.x);
        }

        if (targetXAngle > -1f)
        {
            targetRotation.x = 0;
        }
    }
}
