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

    private void Awake()
    {
        SetProperties();
        gun = properties.transform.GetComponent<Gun>();
    }

    public Vector3 ApplyRecoil()
    {
        if (properties != null)
        {
            float returnSpeed = properties.returnSpeed;
            float snappiness = properties.snappiness;

            if (!gun.IsShooting)
            {
                targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);

                if (targetRotation.Round(0) == Vector3.zero)
                {
                    targetRotation = Vector3.zero;
                }
            }

            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

            if(currentRotation.x > -0.1)
            {
                currentRotation = Vector3.zero;
            }
        }

        if (properties == null || !properties.transform.IsChildOf(transform))
        {
            SetProperties();
        }

        if(properties != null && (gun == null || !gun.transform.IsChildOf(transform)))
        {
            gun = properties.transform.GetComponent<Gun>();
        }

        return currentRotation;
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
    }
}
