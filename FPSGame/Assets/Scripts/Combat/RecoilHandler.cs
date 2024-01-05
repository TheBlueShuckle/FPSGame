using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RecoilHandler : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private RecoilProperties properties;
    [SerializeField] private Camera cam;

    void Update()
    {
        if (properties != null)
        {
            float returnSpeed = properties.returnSpeed;
            float snappiness = properties.snappiness;

            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
            transform.localRotation = Quaternion.Euler(currentRotation);
        }

        if (properties == null || !properties.transform.IsChildOf(transform))
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
    }

    public void Recoil()
    {
        float recoilX = properties.recoilX;
        float recoilY = Random.Range(-properties.recoilY, properties.recoilY);
        float recoilZ = Random.Range(-properties.recoilZ, properties.recoilZ);

        targetRotation += new Vector3(recoilX, recoilY, recoilZ);
    }
}
