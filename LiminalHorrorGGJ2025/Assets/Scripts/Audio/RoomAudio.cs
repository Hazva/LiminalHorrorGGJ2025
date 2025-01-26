using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAudio : MonoBehaviour
{
    public AK.Wwise.State room;

    public void SetRoomState()
    {
        room.SetValue();
    }
}
