using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Surface
    {
        Tile,
        Carpet,
        Grass,
        Shallow_Water
    }

    public static AudioManager Instance;
    public int stressLevel = 1;
    public Surface surface = Surface.Tile;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetState("Movement", "Walk");
        AkSoundEngine.SetState("Surface", "Tile");
        AkSoundEngine.SetState("Room", "Subway");
        AkSoundEngine.SetState("Stress", "Small");

        PostEvent("Play_Ambience");
        PostEvent("Play_Breathing");
    }

    public void PostEvent(string eventName, GameObject emitter = null)
    {
        if (emitter == null)
        {
            AkSoundEngine.PostEvent(eventName, gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent(eventName, emitter);
        }
    }
}
