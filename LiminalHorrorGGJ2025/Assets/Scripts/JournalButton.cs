using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButton : MonoBehaviour
{
    public GameObject[] objectsToDeactivate;
    public GameObject[] objectsToActivate;
    public GameObject playerObj;
    public GameObject bottle;
    public float distThreshold;
    private bool first = true;
    void Update()
    {
        if (Vector3.Distance(playerObj.transform.position, bottle.transform.position) > distThreshold && first)
        {
            foreach (GameObject gm in objectsToActivate)
            {
                gm.SetActive(true);
                AudioManager.Instance.PostEvent("Play_Lights_Off");
            }
            foreach (GameObject gm in objectsToDeactivate)
            {
                gm.SetActive(false);
            }

            first = false;
        }
    }

    private void OnEnable()
    {
        first = true;
        AudioManager.Instance.PostEvent("Play_sfx_bottle_open");
    }
}
