using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBack : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 newPosition = new Vector3(teleportDestination.position.x, teleportDestination.position.y, other.transform.position.z);
            Debug.Log("Teleporting");
            other.transform.position = newPosition;
        }
    }
}
