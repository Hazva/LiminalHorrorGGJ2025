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

    private int _currLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        transitionTrigger.teleportObject = teleportPoints[_currLevel];   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currLevel == INFINITE_ROOM_LEVEL_INDEX)
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
            _currLevel = (_currLevel + 1) % teleportPoints.Length;
            transitionTrigger.teleportObject = teleportPoints[_currLevel];
        }
    }
}
