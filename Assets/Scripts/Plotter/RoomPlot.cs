using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlot : MonoBehaviour {

    public Room data;
    public SpriteRenderer sprite;

    public void Highlight() {
        sprite.color = new Color(46, 204, 113, 1);
    }
    
}
