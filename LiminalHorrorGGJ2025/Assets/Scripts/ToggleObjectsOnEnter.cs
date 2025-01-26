using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectsOnEnter : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    public bool hasTriggered = false;
    public void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered)
        {
            foreach (GameObject gameObject in objectsToActivate)
            {
                gameObject.SetActive(true);
            }
            foreach (GameObject gameObject in objectsToDeactivate)
            {
                gameObject.SetActive(false);
            }
            hasTriggered = true;
        }
    }
}
