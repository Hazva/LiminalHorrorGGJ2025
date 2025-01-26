using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmersionDetector : MonoBehaviour
{
    [SerializeField] private Canvas submergedCanvas; 
    private Collider waterCollider; 
    private bool isSubmerged = false; // Tracks if the player is fully submerged

    private void Start()
    {
        // Ensure the canvas is initially inactive
        if (submergedCanvas != null)
        {
            submergedCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterCollider = other; // Store the water collider
            CheckSubmersion();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            CheckSubmersion();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            CheckSubmersion();
        }
    }

    private void CheckSubmersion()
    {
        if (waterCollider != null)
        {
            // Check if the player's bounds are fully inside the water's bounds
            Bounds waterBounds = waterCollider.bounds;
            Bounds playerBounds = GetComponent<Collider>().bounds;

            isSubmerged = waterBounds.Contains(playerBounds.min) && waterBounds.Contains(playerBounds.max);

            // Toggle the canvas based on submersion status
            if (submergedCanvas != null)
            {
                submergedCanvas.gameObject.SetActive(isSubmerged);
            }
        }
    }
}