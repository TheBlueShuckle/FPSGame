using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEditorInternal;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smoothness;
    [SerializeField] private float swayMultiplier;

    public void Sway(Vector2 input)
    {
        input *= 0.5f * 0.1f;

        float mouseX = input.x * swayMultiplier;
        float mouseY = input.y * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothness * Time.deltaTime);
    }
}
