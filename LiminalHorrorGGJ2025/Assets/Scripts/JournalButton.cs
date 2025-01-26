using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButton : MonoBehaviour
{
    public GameObject[] objectsToDeactivate;
    public GameObject[] objectsToActivate;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject gm in objectsToActivate)
            {
                gm.SetActive(true);
            }
            foreach (GameObject gm in objectsToDeactivate)
            {
                gm.SetActive(false);
            }
        }
        
    }
}
