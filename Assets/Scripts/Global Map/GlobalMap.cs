using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMap : MonoBehaviour {

    public int height = 13, width = 13;
    public float tileSize = 1;

    private int counter = -1;
    [SerializeField] private GameObject tilePrefab;

    void Start() {
        for (int y = height; y > 0; y--) {
            for (int x = 0; x < width; x++) {
                Vector3 tilePosition = new Vector3(x * tileSize, y * tileSize, 0) + Vector3.zero;

                // make a flat cube as a "tile"
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.SetParent(this.transform);
                tile.transform.position = tilePosition;

                counter ++;
                tile.name = $"{counter}";

                Destroy(tile.GetComponent<TileData>());
            }
        }


        Camera.main.transform.position = new Vector3 (  (width * tileSize)/2 - (tileSize/2),
                                                        (height * tileSize)/2 + (tileSize/2),
                                                        Camera.main.transform.position.z
                                                     );
        
    }

}