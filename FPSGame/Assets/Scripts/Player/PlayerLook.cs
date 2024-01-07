using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    public RecoilHandler recoil;
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
        Vector2 magicFix = input * 0.05f;
        Vector3 recoilVector = recoil.ApplyRecoil();

        float mouseX = magicFix.x;
        float mouseY = magicFix.y;

        xRotation -= mouseY * ySensitivity;

        // incase you're looking down while shooting and xRotation is capped so you can't look down.
        if (Mathf.Clamp(xRotation, -90, 90) == 90 && recoilVector.x < 0)
        {
            float maxClamp = 90 - recoilVector.x;
            xRotation = Mathf.Clamp(xRotation, -90, maxClamp);
        }

        else
        {
            xRotation = Mathf.Clamp(xRotation, -90, 90);
        }

        if (xRotation + recoilVector.x < -90)
        {
            recoilVector.x = 0;
            xRotation = -90;
        }

        cam.transform.localEulerAngles = new Vector3(xRotation, 0, 0) + recoilVector;
        transform.Rotate(mouseX * xSensitivity * Vector3.up);
    }
}
