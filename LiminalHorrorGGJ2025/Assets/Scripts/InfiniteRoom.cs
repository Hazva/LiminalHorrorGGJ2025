using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoom : MonoBehaviour
{
    public static InfiniteRoom Instance { get; private set; }
    [SerializeField] private float width;
    [SerializeField] private GameObject roomPrefab;
    public GameObject currentRoom = null;

    private List<GameObject> rooms = new();
    private List<bool> roomsInRange = new();
    public bool bGenerateRooms = false;

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (bGenerateRooms)
        {
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
}
