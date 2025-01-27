using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AK.Wwise.State room;
    public bool bIsDebug = true;

    public enum Surface
    {
        Tile,
        Carpet,
        Grass,
        Shallow_Water
    }

    public static AudioManager Instance;
    public int stressLevel = 0;
    public Surface surface = Surface.Tile;
    public bool isSwimming = false;

    private void Awake()
    {
        if (Instance != null)
        {
            if (!bIsDebug)
            {
                Destroy(Instance.gameObject);
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
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
        room.SetValue();
        AkSoundEngine.SetState("Stress", "Small");
        AkSoundEngine.SetState("Submerged", "NonSubmerged");

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
