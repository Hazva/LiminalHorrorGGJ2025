using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAndWaterController : MonoBehaviour
{
    [SerializeField] private Light[] lightsToToggle; // array of lights to toggle
    [SerializeField] private GameObject[] waterObjects; 
    [SerializeField] private float toggleInterval = 0.5f; // time interval for toggling lights
    [SerializeField] private float waterScaleSpeed = 0.5f; // speed at which the water scales
    [SerializeField] private float maxWaterScale = 10f; // max vertical scale of the water
    private bool isActive = false; // if the effect is active
    private bool coroutineRunning = false;
    private float timer = 0f;
    private float randomInterval = 2.0f;

    private void Update()
    {
        if (isActive)
        {
            if (waterObjects != null)
            {
                foreach (GameObject gm in waterObjects)
                {
                    Vector3 currentScale = gm.transform.localScale;
                    if (currentScale.y < maxWaterScale)
                    {
                        currentScale.y += waterScaleSpeed * Time.deltaTime;
                        gm.transform.localScale = currentScale;
                    }
                }
                
            }
            if (!coroutineRunning)
            {
                // Increment the timer with the time elapsed since the last frame
                timer += Time.deltaTime;

                // Check if the timer has reached the random interval
                if (timer >= randomInterval)
                {
                    timer = 0f; // Reset the timer
                    randomInterval = Random.Range(2f, 2.5f); // Set a new random interval
                    StartCoroutine(FlashCoroutine()); // Trigger the coroutine
                }
            }
        }
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            //StartCoroutine(ToggleLights());
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
            toggleInterval = Random.Range(0.2f, 0.8f);
            yield return new WaitForSeconds(toggleInterval); // Wait before toggling again
        }
    }

    private IEnumerator FlashCoroutine()
    {
        coroutineRunning = true;

        for (int i = 0; i < 6; i++)
        {
            foreach (Light light in lightsToToggle)
            {
                if (light != null)
                {
                    light.enabled = !light.enabled;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        coroutineRunning = false;
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

    public void OnEnable()
    {
        Activate();
    }
}
