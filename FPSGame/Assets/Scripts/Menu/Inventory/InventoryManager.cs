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

    public bool isOpen = false;

    private void Awake()
    {
        inventory.OnCardLeftClickedEvent += Equip;
        cardPanel.OnCardLeftClickedEvent += Unequip;
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

    public void OpenInventory()
    {
        inventoryGO.SetActive(true);
        postProcessing.SetActive(true);
        inGameUI.SetActive(false);
        isOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseInventory()
    {
        inventoryGO.SetActive(false);
        postProcessing.SetActive(false);
        inGameUI.SetActive(true);
        isOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
