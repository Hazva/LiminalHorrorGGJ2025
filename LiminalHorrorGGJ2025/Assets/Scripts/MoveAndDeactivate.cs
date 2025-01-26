using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MoveAndDeactivate : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation; 
    [SerializeField] private Transform secondLocation;
    [SerializeField] private Transform thirdLocation;
    [SerializeField] private Transform finalLocation;
    [SerializeField] private GameObject objectToMove; // GameObject to be moved
    [SerializeField] private float moveSpeed = 5f; // Speed at which the GameObject moves

    public bool isProcessing = false; // Prevents multiple triggers at once

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isProcessing)
        {
            AudioManager.Instance.PostEvent("Play_Jumpscare");
            StartCoroutine(HandleObjectMovement());
        }
    }

    private IEnumerator HandleObjectMovement()
    {
        if (objectToMove != null && spawnLocation != null && secondLocation != null && finalLocation != null)
        {
            isProcessing = true;

            objectToMove.transform.position = spawnLocation.position;
            objectToMove.SetActive(true);

            yield return StartCoroutine(MoveObjectToPosition(objectToMove, secondLocation.position));

            yield return StartCoroutine(MoveObjectToPosition(objectToMove, thirdLocation.position));

            yield return StartCoroutine(MoveObjectToPosition(objectToMove, finalLocation.position));

            // Deactivate the GameObject
            objectToMove.SetActive(false);

            isProcessing = false;
        }
    }

    private IEnumerator MoveObjectToPosition(GameObject obj, Vector3 targetPosition)
    {
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Snap to the target position
        obj.transform.position = targetPosition;
    }
}