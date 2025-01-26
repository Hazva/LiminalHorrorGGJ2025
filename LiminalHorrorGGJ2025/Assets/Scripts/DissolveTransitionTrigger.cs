using Coffee.UIEffects;
using System.Collections;
using UnityEngine;

public class DissolveTransitionTrigger : MonoBehaviour
{
    public UIEffect transitionController;
    public GameObject teleportObject;
    public GameObject renderTexCamera;

    private Coroutine transitionCoroutine;
    private CharacterController characterController;

    private void Start()
    {
        Reset();

        characterController = GetComponent<CharacterController>();
    }

    public void TriggerTransition()
    {
        renderTexCamera.SetActive(false);

        // Manually handle CharacterController physics and teleport
        characterController.enabled = false;
        transform.position = teleportObject.transform.position;
        characterController.enabled = true;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        // Start the overlapping transition coroutine
        transitionCoroutine = StartCoroutine(LerpTransition(0.5f, 1.0f, 0.3f));
    }

    private void Reset()
    {
        transitionController.samplingIntensity = 0.0f;
        transitionController.transitionRate = 0.0f;
    }

    private IEnumerator LerpTransition(float durationA, float durationB, float overlap)
    {
        float elapsedTimeA = 0.0f;
        float elapsedTimeB = -overlap; // Start B's timer with an overlap delay
        float startSamplingIntensity = transitionController.samplingIntensity;
        float startTransitionRate = transitionController.transitionRate;
        float targetValue = 1.0f;

        AudioManager.Instance.PostEvent("Play_Transition");

        while (elapsedTimeA < durationA || elapsedTimeB < durationB)
        {
            if (elapsedTimeA < durationA)
            {
                elapsedTimeA += Time.deltaTime;
                float tA = Mathf.Clamp01(elapsedTimeA / durationA);
                transitionController.samplingIntensity = Mathf.Lerp(startSamplingIntensity, targetValue, tA);
            }

            if (elapsedTimeB >= 0 && elapsedTimeB < durationB)
            {
                elapsedTimeB += Time.deltaTime;
                float tB = Mathf.Clamp01(elapsedTimeB / durationB);
                transitionController.transitionRate = Mathf.Lerp(startTransitionRate, targetValue, tB);
            }
            else
            {
                elapsedTimeB += Time.deltaTime;
            }

            yield return null;
        }
        teleportObject.GetComponent<RoomAudio>().SetRoomState();

        // Ensure both values are set to their target at the end
        transitionController.samplingIntensity = targetValue;
        transitionController.transitionRate = targetValue;

        renderTexCamera.SetActive(true);
        Reset();
        transitionCoroutine = null;
    }
}
