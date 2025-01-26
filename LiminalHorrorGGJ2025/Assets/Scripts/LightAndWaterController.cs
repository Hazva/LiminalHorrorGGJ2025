using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAndWaterController : MonoBehaviour
{
    [SerializeField] private Light[] lightsToToggle; // array of lights to toggle
    [SerializeField] private GameObject waterObject; 
    [SerializeField] private float toggleInterval = 0.5f; // time interval for toggling lights
    [SerializeField] private float waterScaleSpeed = 0.5f; // speed at which the water scales
    [SerializeField] private float maxWaterScale = 10f; // max vertical scale of the water
    private bool isActive = false; // if the effect is active
    private float waterInitialScale; // store the initial Y scale of the water

    private void Start()
    {
        // Store the initial scale of the water object
        if (waterObject != null)
        {
            waterInitialScale = waterObject.transform.localScale.y;
        }
    }

    private void Update()
    {
        if (isActive)
        {
            // Gradually scale up the water object
            if (waterObject != null)
            {
                Vector3 currentScale = waterObject.transform.localScale;
                if (currentScale.y < maxWaterScale)
                {
                    currentScale.y += waterScaleSpeed * Time.deltaTime;
                    waterObject.transform.localScale = currentScale;
                }
            }
        }
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(ToggleLights());
        }
    }

    private System.Collections.IEnumerator ToggleLights()
    {
        while (isActive)
        {
            // Toggle each light on and off
            foreach (Light light in lightsToToggle)
            {
                if (light != null)
                {
                    light.enabled = !light.enabled;
                }
            }
            yield return new WaitForSeconds(toggleInterval); // Wait before toggling again
        }
    }

    public void Deactivate()
    {
        isActive = false;

        // Ensure all lights are turned off when deactivated
        foreach (Light light in lightsToToggle)
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }
    }
}
