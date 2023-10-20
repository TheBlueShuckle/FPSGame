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
    [SerializeField]
    private TextMeshProUGUI pointText;
    [SerializeField]
    private TextMeshProUGUI currentRoundText;
    [SerializeField]
    private GameObject roundHandlerGO;

    private Gun gun;
    private RoundHandler roundHandler;

    private void Start()
    {
        if (gameObject.GetComponentInChildren<Gun>() != null)
        {
            gun = gameObject.GetComponentInChildren<Gun>();
        }

        roundHandler = roundHandlerGO.GetComponent<RoundHandler>();

        PointCounter.ResetPoints();
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
        UpdatePointText();
        UpdateCurrentRoundText();
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

    public void UpdatePointText()
    {
        pointText.text = $"Points: {PointCounter.Points}";
    }

    public void UpdateCurrentRoundText()
    {
        currentRoundText.text = $"Current Round: {roundHandler.CurrentRound}";
    }
}
