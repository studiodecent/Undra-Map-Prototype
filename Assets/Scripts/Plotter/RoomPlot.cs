using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlot : MonoBehaviour {

    public Room data;
    public SpriteRenderer sprite;

    private Color colour;

    public void SetData(Room roomData) {
        data = roomData;
        this.name = data.id;

        // this isn't working???
        colour = GetRoomColour(data.type);
        this.sprite.material.color = colour;
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

    private void OnMouseEnter() {
        this.sprite.material.color = Color.white;
        GameObject.FindWithTag("GUI").GetComponent<CanvasController>().DisplayInfo(data);
    }

    private void OnMouseExit() {
        this.sprite.material.color = colour;
        GameObject.FindWithTag("GUI").GetComponent<CanvasController>().DisplayInfo();
    }

    
    
}
