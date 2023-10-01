using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField]
    private TextMeshProUGUI ammoText;

    private Gun gun;

    private void Start()
    {
        gun = gameObject.GetComponentInChildren<Gun>();

    }

    private void Update()
    {
        UpdateAmmoText();
    }

    public void UpdatePromptMessage(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void UpdateAmmoText()
    {
        ammoText.text = gun.GetAmmoLeftRatio();
    }
}
