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
    public PaperProximity paperProx;
    public GameObject creditsObject;
    public GameObject renderTexObj;

    private const int INFINITE_ROOM_LEVEL_INDEX = 0;
    private const int WHITE_ROOM_LEVEL_INDEX = 1;
    public static GameManager Instance { get; private set; }

    public int currLevel = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        transitionTrigger.teleportObject = teleportPoints[currLevel];   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }
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

        if (currLevel == WHITE_ROOM_LEVEL_INDEX)
        {
            paperProx.enabled = true;
        }
        else
        {
            paperProx.enabled = false;
        }

        transitionTrigger.TriggerTransition();
        currLevel = (currLevel + 1) % teleportPoints.Length;
        transitionTrigger.teleportObject = teleportPoints[currLevel];
        if (currLevel == 0)
        {
            StartCoroutine(CreditsDelay());
        }
    }

    IEnumerator CreditsDelay()
    {
        yield return new WaitForSeconds(5.0f);
        renderTexObj.SetActive(false);
        creditsObject.SetActive(true);
    }

    public void StartInfiniteRoomPuzzle()
    {
        infiniteRoomManager.StartPuzzle();
    }
}
