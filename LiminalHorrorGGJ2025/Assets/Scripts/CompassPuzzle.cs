using System.Collections;
using UnityEngine;

public class CompassPuzzle : MonoBehaviour
{
    public GameObject compassImage;
    public GameObject target;
    public bool puzzleSolved = false;
    private bool snapped = true;

    private Vector3 initialVelocity;
    private Vector3 currentVelocity;
    private Vector2 lastDirection = Vector2.zero;
    [SerializeField]
    private float directionHoldTime = 0f;
    private const float targetHoldTime = 10f;
    private const float decelerationRate = 0.15f;

    void Start()
    {
        initialVelocity = new Vector3(Random.Range(400f, 500f), Random.Range(400f, 500f), Random.Range(400f, 500f));
        currentVelocity = initialVelocity;
        AudioManager.Instance.PostEvent("Play_Compass_Spin");
    }

    void Update()
    {
        if (!puzzleSolved)
        {
            AkSoundEngine.SetRTPCValue("Compass_Velocity", currentVelocity.magnitude);
            compassImage.GetComponent<RectTransform>().Rotate(currentVelocity * Time.deltaTime);

            // Get player input direction
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;

            if (inputDirection != Vector2.zero)
            {
                // Check if the player has changed direction
                if (Vector2.Dot(inputDirection, lastDirection) < 0.99f)
                {
                    // Reset hold time if direction changes significantly
                    directionHoldTime = 0f;
                    lastDirection = inputDirection;
                    currentVelocity = initialVelocity;
                }
                else
                {
                    // Increment hold time if direction is consistent
                    directionHoldTime += Time.deltaTime;
                }

                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
                targetAngle = (targetAngle + 360f) % 360f; // Ensure angle is between 0 and 360 degrees

                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                currentVelocity = Vector3.Lerp(currentVelocity, new Vector3(50f, 50f, 50f), Time.deltaTime * decelerationRate);

                compassImage.GetComponent<RectTransform>().localRotation = Quaternion.Lerp(compassImage.GetComponent<RectTransform>().localRotation, targetRotation, Time.deltaTime * 1.5f);
            }
            else
            {
                currentVelocity = Vector3.Lerp(currentVelocity, initialVelocity, Time.deltaTime * 0.2f);
                directionHoldTime = 0f; // Reset hold time
            }

            if (directionHoldTime >= targetHoldTime)
            {
                puzzleSolved = true;
                StartCoroutine(SnapToSolvedRotation());
            }
        }
        else if (snapped)
        {
            if (target != null)
            {
                Vector3 directionToTarget = target.transform.position - gameObject.transform.position;
                directionToTarget.Normalize();

                float angleToTarget = Vector3.SignedAngle(gameObject.transform.forward, directionToTarget, Vector3.up);
                Debug.Log(angleToTarget);

                angleToTarget = (angleToTarget + 360f) % 360f; 

                Quaternion targetRotation = Quaternion.Euler(0, 0, -angleToTarget);
                compassImage.GetComponent<RectTransform>().localRotation = targetRotation;
            }
        }
    }

    private IEnumerator SnapToSolvedRotation()
    {
        
        float solvedAngle = Mathf.Atan2(lastDirection.x, lastDirection.y) * Mathf.Rad2Deg;
        solvedAngle = (solvedAngle + 360f) % 360f; 
        Quaternion startRotation = compassImage.GetComponent<RectTransform>().localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, -solvedAngle);
        Debug.Log(solvedAngle);
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            compassImage.GetComponent<RectTransform>().localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        AudioManager.Instance.PostEvent("Stop_Compass_Spin");
        AudioManager.Instance.PostEvent("Play_Compass_Pickup");
        snapped = true;
    }
}
