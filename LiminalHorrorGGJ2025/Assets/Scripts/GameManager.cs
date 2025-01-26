using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] teleportPoints;
    public DissolveTransitionTrigger transitionTrigger;
    public InfiniteRoom infiniteRoomManager;
    public GameObject fogObject;
    public GameObject compassImage;

    private const int INFINITE_ROOM_LEVEL_INDEX = 0;
    public static GameManager Instance { get; private set; }

    private int currLevel = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        transitionTrigger.teleportObject = teleportPoints[currLevel];   
    }

    // Update is called once per frame
    public void NextLevel()
    {
        if (currLevel == INFINITE_ROOM_LEVEL_INDEX)
        {
            infiniteRoomManager.enabled = true;
            fogObject.SetActive(true);
        }
        else
        {
            infiniteRoomManager.enabled = false;
            fogObject.SetActive(false);
            compassImage.SetActive(false);
        }

        transitionTrigger.TriggerTransition();
        currLevel = (currLevel + 1) % teleportPoints.Length;
        transitionTrigger.teleportObject = teleportPoints[currLevel];
    }

    public void StartInfiniteRoomPuzzle()
    {
        infiniteRoomManager.StartPuzzle();
    }
}
