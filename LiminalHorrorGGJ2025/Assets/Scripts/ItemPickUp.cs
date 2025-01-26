using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    public static ItemPickUp Instance { get; private set; }

    public Sprite[] journalSprites;
    public GameManager manager;
    public Image canvasImage;
    public GameObject canvasImageObj;

    private Coroutine currentFadeCoroutine;
    public bool isImageTriggered = false;
    [SerializeField]
    private bool first = true;
    [SerializeField]
    private int currLevel = 0;

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

    // make sure levels are 0 indexed
    public void TriggerImageForLevel(int level)
    {
        if (isImageTriggered) { return; }
        canvasImageObj.SetActive(true);
        if (level != currLevel)
        {
            first = true;
            currLevel = level;
        }

        if (level < 0 || level >= journalSprites.Length) return;

        canvasImage.sprite = journalSprites[level];
        if (level == 0)
        {
            AudioManager.Instance.PostEvent("Play_sfx_bottle_open");
        }

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeImage(canvasImage, 0f, 1f, 0.5f));
        Debug.Log("Triggered");
        isImageTriggered = true;
    }

    public void UntriggerCurrentImage()
    {
        if (!isImageTriggered) return;

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        if (currLevel == 1 && first)
        {
            manager.StartInfiniteRoomPuzzle();
            first = false;
        }

        currentFadeCoroutine = StartCoroutine(FadeImage(canvasImage, 1f, 0f, 0.5f));
        isImageTriggered = false;
        canvasImageObj.SetActive(false);
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
