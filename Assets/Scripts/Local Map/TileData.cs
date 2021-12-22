using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour {

    public Room data;
    [Space]
    private string id, owner = "unclaimed", type, rarity;
    private Vector2 coords;
    [Space]
    public Color uncommonColour;
    public Color rareColor;
    public Color epicColor;
    public Color legendaryColor;

    private void Start() {

        if (data != null) {
            this.id = data.id;
            if (data.owner != null) this.owner = data.owner;
            this.type = data.type;
            this.rarity = data.rarity;
            this.coords = new Vector2(data.coords.x, data.coords.y);
        }

        Material material = GetComponent<MeshRenderer>().material;

        switch (rarity) {
            case "uncommon":
                material.color = uncommonColour;
                break;
            case "rare":
                material.color = rareColor;
                break;
            case "epic":
                material.color = epicColor;
                break;
            case "legendary":
                material.color = legendaryColor;
                break;
            default:
                material.color = Color.white;
                GetComponent<Collider>().enabled = false;
                break;
        }
    }

}
