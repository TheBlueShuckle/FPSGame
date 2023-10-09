using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : Interactable
{
    [SerializeField] GameObject gunPrefab;
    PickUpController pickUpController;
    GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    protected override void Interact()
    {
        Vector3 playerPosition = player.transform.position;
        GameObject gun = Instantiate(gunPrefab, new Vector3(playerPosition.x, playerPosition.y + 1, playerPosition.z), Quaternion.identity);

        AutoPickUp(gun);
    }

    private void AutoPickUp(GameObject gun)
    {
        if ((pickUpController = gun.GetComponent<PickUpController>()) != null)
        {
            if (!pickUpController.equipped)
            {
                pickUpController.PickUp();
            }
        }
    }
}
