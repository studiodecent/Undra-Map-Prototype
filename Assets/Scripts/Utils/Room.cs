using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room {
    public string id, owner, type, rarity;
    public Vector2 coords;
}

[System.Serializable]
public class RoomList {
    public Room[] rooms;

    public RoomList(int size) {
        rooms = new Room[size];
    }

}