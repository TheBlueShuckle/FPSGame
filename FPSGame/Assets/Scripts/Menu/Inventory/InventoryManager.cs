using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject inventoryGO;
    [SerializeField] GameObject postProcessing;
    [SerializeField] GameObject inGameUI;
    [SerializeField] Inventory inventory;
    [SerializeField] CardPanel cardPanel;

    private PlayerInput playerInputs;
    private InputAction menu;

    public bool isOpen = false;

    private void Awake()
    {
        inventory.OnCardLeftClickedEvent += Equip;
        cardPanel.OnCardLeftClickedEvent += Unequip;

        playerInputs = new PlayerInput();
    }

    public void Equip(Card card)
    {
        if (card.tempIsUnlocked && !card.tempIsEquipped)
        {
            cardPanel.AddCard(card);
            print("Equipped " + card.name);
        }
    }

    public void Unequip(Card card)
    {
        print("Unequipped " + card.name);

        cardPanel.RemoveCard(card);
    }

    public void ToggleInventory()
    {
        inventoryGO.SetActive(!inventoryGO.activeSelf);
        postProcessing.SetActive(inventoryGO.activeSelf);
        inGameUI.SetActive(!inventoryGO.activeSelf);
        isOpen = inventoryGO.activeSelf;

        if (inventoryGO.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
