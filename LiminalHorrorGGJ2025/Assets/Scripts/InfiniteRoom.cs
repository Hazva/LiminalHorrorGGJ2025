using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoom : MonoBehaviour
{
    public static InfiniteRoom Instance { get; private set; }
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject fog;
    [SerializeField] private ParticleSystem[] fogParticles;
    public GameObject currentRoom = null;
    public CompassPuzzle compassPuzzle;

    private List<GameObject> rooms = new();
    private List<bool> roomsInRange = new();
    public bool bGenerateRooms = false;
    [SerializeField] private float doorDistance = 80.0f;

    private float timer = 5f;
    public GameObject compassPrefab;
    public float spawnDistance;
    public CharacterController charController;
    public GameObject playerObj;

    private bool puzzleStarted = false;
    private GameObject lastSpawnedPrefab = null;
    private Coroutine respawnCoroutine = null;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerMoving() && timer > 0 && !puzzleStarted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnPrefabInFront();
            }
            else if (timer <= 2)
            {
                AudioManager.Instance.stressLevel = 3;
                AkSoundEngine.SetState("Stress", "Large");
            }
            else if (timer <= 4)
            {
                AudioManager.Instance.stressLevel = 2;
                AkSoundEngine.SetState("Stress", "Med");
            }
            else
            {
                AudioManager.Instance.stressLevel = 1;
                AkSoundEngine.SetState("Stress", "Small");
            }
        }

        if (bGenerateRooms)
        {
            /*
            if (Input.GetKeyDown(KeyCode.K))
            {
                SpawnDoor();
                StopGeneration();
                return;
            }
            */

            for (int i = 0; i < roomsInRange.Count; i++)
            {
                roomsInRange[i] = false;
            }

            Vector3 pos = SC_FPSController.Instance.transform.position;
            if (currentRoom == null)
            {
                currentRoom = Instantiate(roomPrefab, pos, Quaternion.identity);
                rooms.Add(currentRoom);
                roomsInRange.Add(true);
            }
            
            for (int x = -1; x < 2; x++)
            {
                for (int z = -1; z < 2; z++)
                {
                    Vector3 offset = new Vector3(x * width, 0, z * width);
                    Vector3 roomPos = currentRoom.transform.position + offset;
                    if (!RoomExists(roomPos))
                    {
                        GameObject room = Instantiate(roomPrefab, roomPos, Quaternion.identity);
                        rooms.Add(room);
                        roomsInRange.Add(true);
                    }
                }
            }

            for (int i = rooms.Count - 1; i >= 0; i--)
            {
                if (!roomsInRange[i])
                {
                    Destroy(rooms[i]);
                    rooms.RemoveAt(i);
                    roomsInRange.RemoveAt(i);
                }
            }

            //Debug.Log("Rooms spawned: " + rooms.Count);
        }
    }

    public void StartPuzzle()
    {
        compassPuzzle.enabled = true;
        puzzleStarted = true;
    }

    bool RoomExists(Vector3 pos)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].transform.position == pos)
            {
                roomsInRange[i] = true;
                return true;
            }
        }
        return false;
    }

    public GameObject SpawnDoor()
    {
        Vector3 spawnPos = SC_FPSController.Instance.transform.position + SC_FPSController.Instance.moveDirection.normalized * doorDistance;
        spawnPos.y = currentRoom.transform.position.y + height / 2.0f;
        return Instantiate(doorPrefab, spawnPos, Quaternion.identity);
    }

    public void StopGeneration()
    {
        if (bGenerateRooms)
        {
            bGenerateRooms = false;
            StartCoroutine(DispellFog());
            Quaternion rot = Quaternion.Euler(0, 90, 0);
            Instantiate(wallPrefab, currentRoom.transform.position + new Vector3(-2 * width, height / 2.0f, 0), rot);
            Instantiate(wallPrefab, currentRoom.transform.position + new Vector3(2 * width, height / 2.0f, 0), rot);
            Instantiate(wallPrefab, currentRoom.transform.position + new Vector3(0, height / 2.0f, -2 * width), Quaternion.identity);
            Instantiate(wallPrefab, currentRoom.transform.position + new Vector3(0, height / 2.0f, 2 * width), Quaternion.identity);
        }
        
    }

    IEnumerator DispellFog()
    {
        AudioManager.Instance.PostEvent("Dispel_Smoke");

        var emission1 = fogParticles[0].main;
        var emission2 = fogParticles[1].main;
        var emission3 = fogParticles[2].main;
        var emission4 = fogParticles[3].main;

        emission1.startLifetimeMultiplier = 0f;
        emission2.startLifetimeMultiplier = 0f;
        emission3.startLifetimeMultiplier = 0f;
        emission4.startLifetimeMultiplier = 0f;
        yield return new WaitForSeconds(5.0f);

        fog.SetActive(false);
    }
    private bool IsPlayerMoving()
    {
        return charController.velocity.magnitude > 0.1f;
    }

    private void SpawnPrefabInFront()
    {
        if (compassPrefab != null)
        {
            Vector3 forwardGroundAligned = playerObj.transform.forward;
            forwardGroundAligned.y = 0;
            forwardGroundAligned.Normalize();
            Vector3 spawnPosition = playerObj.transform.position + forwardGroundAligned * spawnDistance;

            if (lastSpawnedPrefab != null)
            {
                Destroy(lastSpawnedPrefab);
            }

            lastSpawnedPrefab = Instantiate(compassPrefab, spawnPosition, Quaternion.identity);
            lastSpawnedPrefab.SetActive(true);

            if (respawnCoroutine != null)
            {
                StopCoroutine(respawnCoroutine);
            }
            respawnCoroutine = StartCoroutine(RespawnAfterInactivity());
        }
    }

    private IEnumerator RespawnAfterInactivity()
    {
        yield return new WaitForSeconds(20f);

        if (lastSpawnedPrefab != null && !puzzleStarted && !ItemPickUp.Instance.isImageTriggered)
        {
            Destroy(lastSpawnedPrefab);
            lastSpawnedPrefab = null;
            SpawnPrefabInFront();
        }
    }
}
