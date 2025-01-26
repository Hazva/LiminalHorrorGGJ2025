using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoomTracker : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InfiniteRoom.Instance.currentRoom = gameObject;
        }
    }
}
