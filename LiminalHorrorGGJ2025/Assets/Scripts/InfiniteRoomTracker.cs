using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoomTracker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered");
            InfiniteRoom.Instance.currentRoom = gameObject;
        }
    }
}
