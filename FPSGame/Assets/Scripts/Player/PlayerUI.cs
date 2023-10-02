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
        if (gameObject.GetComponentInChildren<Gun>() != null)
        {
            gun = gameObject.GetComponentInChildren<Gun>();
        }
    }

    private void Update()
    {
        if (gun != gameObject.GetComponentInChildren<Gun>())
        {
            if (gameObject.GetComponentInChildren<Gun>() != null)
            {
                gun = gameObject.GetComponentInChildren<Gun>();
            }

            else
            {
                gun = null;
            }
        }

        UpdateAmmoText();
    }

    public void UpdatePromptMessage(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void UpdateAmmoText()
    {
        if (gun != null)
        {
            ammoText.text = gun.GetAmmoLeftRatio();
            return;
        }

        ammoText.text = "";
    }
}
