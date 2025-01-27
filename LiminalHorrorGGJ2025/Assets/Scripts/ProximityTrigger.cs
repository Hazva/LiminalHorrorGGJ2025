using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public GameObject player;
    public GameManager manager;
    public int level;
    public float distanceThreshold = 10.0f;
    [SerializeField]
    private bool triggered;

    private void Start()
    {
        AudioManager.Instance.PostEvent("Play_Compass_Diegetic", gameObject);
        triggered = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= distanceThreshold && !triggered && manager.currLevel == level)
        {
            ItemPickUp.Instance.TriggerImageForLevel(level);
            triggered = true;
            AudioManager.Instance.PostEvent("Play_Journal");
            AudioManager.Instance.PostEvent("Stop_Compass_Diegetic", gameObject);
            AkSoundEngine.SetState("Room", "Intermediate_Locked");
            AudioManager.Instance.stressLevel = 0;
            AkSoundEngine.SetState("Stress", "Small");
        }
        else if (distance > distanceThreshold && triggered)
        {
            ItemPickUp.Instance.UntriggerCurrentImage();
            triggered = false;
        }
    }
}
