using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    public static ItemPickUp Instance { get; private set; }

    public Sprite[] journalSprites;
    public Image canvasImage;

    private Coroutine currentFadeCoroutine;
    private bool isImageTriggered = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        canvasImage.color = new Color(canvasImage.color.r, canvasImage.color.g, canvasImage.color.b, 0);
    }

    private void Update()
    {
        if (!isImageTriggered)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TriggerImageForLevel(0);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UntriggerCurrentImage();
            }
        }
    }

    // make sure levels are 0 indexed
    public void TriggerImageForLevel(int level)
    {
        if (level < 0 || level >= journalSprites.Length) return;

        canvasImage.sprite = journalSprites[level];
        AudioManager.Instance.PostEvent("Play_sfx_bottle_open");

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeImage(canvasImage, 0f, 1f, 0.5f));
        isImageTriggered = true;
    }

    public void UntriggerCurrentImage()
    {
        if (!isImageTriggered) return;

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeImage(canvasImage, 1f, 0f, 0.5f));
        isImageTriggered = false;
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
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

        color.a = endAlpha;
        image.color = color;

        if (endAlpha == 0f)
        {
            image.sprite = null;
        }
    }
}
