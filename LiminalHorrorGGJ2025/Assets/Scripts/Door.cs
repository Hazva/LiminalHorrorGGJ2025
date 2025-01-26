using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool bActivated = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (!bActivated && collision.gameObject.CompareTag("Player"))
        {
            bActivated = true;
            GameManager.Instance.NextLevel();
        }
    }
}
