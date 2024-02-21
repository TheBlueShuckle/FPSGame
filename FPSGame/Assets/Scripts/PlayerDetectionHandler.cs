using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetectionHandler : MonoBehaviour
{
    [SerializeField] Transform bottomDetector;
    [SerializeField] Transform topDetector;
    [SerializeField] float colliderRadius;
    [SerializeField] Animator topAnimation;
    [SerializeField] Animator bottomAnimation;

    private ElevatorHandler elevator;

    private void Awake()
    {
        elevator = GetComponent<ElevatorHandler>();
    }

    private void Update()
    {
        if (!elevator.IsMoving)
        {
            if (elevator.IsAtTop)
            {
                foreach (Collider collider in Physics.OverlapSphere(topDetector.position, colliderRadius))
                {
                    if (collider.gameObject.CompareTag("Player"))
                    {
                        topAnimation.SetBool("isOpen", true);
                        break;
                    }

                    else
                    {
                        topAnimation.SetBool("isOpen", false);
                    }
                }
            }

            if (!elevator.IsAtTop)
            {
                foreach (Collider collider in Physics.OverlapSphere(bottomDetector.position, colliderRadius))
                {
                    if (collider.gameObject.CompareTag("Player"))
                    {
                        bottomAnimation.SetBool("isOpen", true);
                        break;
                    }

                    else
                    {
                        bottomAnimation.SetBool("isOpen", false);
                    }
                }
            }
        }

        else
        {
            topAnimation.SetBool("isOpen", false);
            bottomAnimation.SetBool("isOpen", false);
        }
    }
}
