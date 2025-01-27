using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightAndWaterController : MonoBehaviour
{
    [SerializeField] private Light[] lightsToToggle; // array of lights to toggle
    [SerializeField] private GameObject[] waterObjects;
    public List<float> waterObjectOriginalScale = new List<float>() {};
    [SerializeField] private float toggleInterval = 0.5f; // time interval for toggling lights
    [SerializeField] private float waterScaleSpeed = 0.5f; // speed at which the water scales
    [SerializeField] private float maxWaterScale = 10f; // max vertical scale of the water
    private bool isActive = false; // if the effect is active
    private bool coroutineRunning = false;
    private float timer = 0f;
    private float randomInterval = 2.0f;
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject player;

    [SerializeField] private float swimmingTime = 8f;
    [SerializeField] private float submersionTime = 16f;
    [SerializeField] private float drowningTime = 35f;
    [SerializeField] private Image targetImage;
    [SerializeField] private float fadeDuration = 3f;
    public GameObject submergedOverlay;
    public GameObject[] objectsToReactivate;
    public ToggleObjectsOnEnter bottle;

    public static LightAndWaterController Instance;

    private void Awake()
    {
        Instance = this;
        if (waterObjects != null)
        {
            foreach (GameObject gm in waterObjects)
            {
                waterObjectOriginalScale.Add(gm.transform.localScale.y);
            }

        }
    }

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

    public void StopCoroutines()
    {
        StopAllCoroutines();
    }

    public void Activate()
    {
        if (!isActive)
        {
            AkSoundEngine.SetState("Room", "Subway_Flood");
            isActive = true;
            //StartCoroutine(ToggleLights());
            StartCoroutine(StartSwimming());
            StartCoroutine(StartSubmersion());
            StartCoroutine(StartDrowning());
        }
    }

    IEnumerator StartSwimming()
    {
        AudioManager.Instance.stressLevel = 3;
        AkSoundEngine.SetState("Stress", "Large");
        yield return new WaitForSeconds(swimmingTime);
        if (isActive)
        {
            AudioManager.Instance.isSwimming = true;
            AkSoundEngine.SetState("Surface", "Water");
        }
        
    }

    IEnumerator StartSubmersion()
    {
        yield return new WaitForSeconds(submersionTime);
        if (isActive)
        {
            AudioManager.Instance.PostEvent("Play_Submerge");
            AkSoundEngine.SetState("Submerged", "Submerged");
            submergedOverlay.SetActive(true);
            
        }
        
    }

    IEnumerator StartDrowning()
    {
        Debug.Log("DROWNING STARTED");
        yield return new WaitForSeconds(drowningTime);
        Debug.Log("Drowning happening");
        StartCoroutine(FadeAlpha(255f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeAlpha(0f));
        yield return new WaitForSeconds(3f);
        Reset();
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
                if (light != null && isActive)
                {
                    light.enabled = !light.enabled;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        coroutineRunning = false;
    }

    public void TurnOnLights()
    {
        coroutineRunning = false;
        isActive = false;
        foreach (Light light in lightsToToggle)
        {
            if (light != null)
            {
                light.enabled = true;
            }
        }
    }

    public void Deactivate()
    {
        isActive = false;

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

    private IEnumerator FadeAlpha(float targetAlpha)
    {
        Color color = targetImage.color;
        float startAlpha = color.a;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            color.a = newAlpha;
            targetImage.color = color;
            yield return null;

        }
        color.a = targetAlpha;
        targetImage.color = color;
    }

    public void Reset()
    {
        int i = 0;
        foreach (GameObject gm in waterObjects)
        {
            Vector3 originalScale = gm.transform.localScale;
            originalScale.y = waterObjectOriginalScale[i];
            gm.transform.localScale = originalScale;
            i++;
        }
        foreach (Light light in lightsToToggle)
        {
            if (light != null)
            {
                light.enabled = true;
            }
        }
        foreach (GameObject gm in objectsToReactivate)
        {
            gm.SetActive(true);
        }
        bottle.hasTriggered = false;
        submergedOverlay.SetActive(false);
        AudioManager.Instance.stressLevel = 0;
        AkSoundEngine.SetState("Stress", "Small");
        AkSoundEngine.SetState("Submerged", "NonSubmerged");
        AkSoundEngine.SetState("Room", "Subway");
        AudioManager.Instance.isSwimming = false;
        characterController.enabled = false;
        player.transform.position = teleportDestination.position;
        characterController.enabled = true;
        //reset audio state here too
        Debug.Log("Reset");
        isActive = false;
        this.gameObject.SetActive(false);
    }
}
