using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperProximity : MonoBehaviour
{
    public Sprite[] journalSprites;
    public GameObject door;
    public GameObject player;
    public Image canvasImage;
    public GameObject canvasImageObj;

    private float startDistance;
    private float primaryThreshold;
    private float secondaryThreshold;
    private int primaryThresholdsReached = 0;
    private int maxPrimaryThresholds = 4;
    private bool isWaitingForSecondary = false;

    private Coroutine currentFadeCoroutine;

    void Start()
    {
        startDistance = Vector3.Distance(player.transform.position, door.transform.position);
        primaryThreshold = startDistance - 15f;
        secondaryThreshold = primaryThreshold - 10f;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, door.transform.position);
        Debug.Log("Distance: " + distance + " Primary Threshold: " + primaryThreshold);

        if (!isWaitingForSecondary && distance <= primaryThreshold && primaryThresholdsReached < maxPrimaryThresholds)
        {
            Debug.Log("HITHITHITHIT");
            ShowPaper();
            isWaitingForSecondary = true;
        }

        if (isWaitingForSecondary && distance <= secondaryThreshold)
        {
            UnshowPaper();
            isWaitingForSecondary = false;

            primaryThreshold -= 15f;
            secondaryThreshold = primaryThreshold - 5f;
        }
    }

    void ShowPaper()
    {
        canvasImageObj.SetActive(true);
        canvasImage.sprite = journalSprites[primaryThresholdsReached];

        currentFadeCoroutine = StartCoroutine(FadeImage(canvasImage, 0f, 1f, 0.5f));
        primaryThresholdsReached++;
    }

    void UnshowPaper()
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeImage(canvasImage, 1f, 0f, 0.5f));
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        Debug.Log("In coroutine");
        if (endAlpha >= 1f)
        {
            canvasImageObj.SetActive(true);
        }

        float elapsedTime = 0f;
        Color color = image.color;
        color.a = startAlpha;
        image.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            color.a = alpha;
            image.color = color;
            yield return null;
        }

        if (endAlpha <= 0f)
        {
            canvasImageObj.SetActive(false);
        }

        color.a = endAlpha;
        image.color = color;

        if (endAlpha == 0f)
        {
            image.sprite = null;
        }
    }
}

