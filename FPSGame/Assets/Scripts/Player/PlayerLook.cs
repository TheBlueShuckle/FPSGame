using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ProcessLook(Vector2 input)
    {
        // magic numbers solves everything!
        Vector2 magicFix = input * 0.5f * 0.1f;

        float mouseX = magicFix.x;
        float mouseY = magicFix.y;

        xRotation -= (mouseY) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX) * xSensitivity);
    }
}
