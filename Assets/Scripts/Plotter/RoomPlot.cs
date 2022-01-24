using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlot : MonoBehaviour {

    public Room data;
    public SpriteRenderer sprite;

    public void SetData(Room roomData) {
        data = roomData;

        // this isn't working?
        sprite.material.color = GetRoomColour(roomData.type);
    }

    private Color GetRoomColour(string type) {
        switch(type) {
            case "site":
                return Color.green;
            case "store":
                return Color.blue;
            case "command":
                return Color.red;
            default:
                return Color.black;
        }
    }

    
    
}
