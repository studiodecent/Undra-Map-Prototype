using System.Collections;
using UnityEngine;

/*
    HELLO JAMIE
    THIS IS PAST JAMIE

    You could also store coordinates as Vector2 values -- do some vector maths to make sure nothing's too close to anything else, etc, and instantiate the "important" tiles at those coordinates on the 10x10 grid.

    It doesn't NEED to be a 10x10 grid, but it's what I'm working with right now.
*/

public class GridSketcher : MonoBehaviour {

    public int width = 10, height = 10;
    public float tileSize = 16;
    [SerializeField] private Vector3 gridOrigin = Vector3.zero;

    [Space]
    [SerializeField] private Color[] colours;

    [Space]
    [SerializeField] private GameObject tilePrefab;
    
    [Space]
    public RoomList tiles;

    private int counter = -1;

    public void CreateGrid(int map) {
        // this generates a grid downwards and rightwards
        // NB might want to take a MapSO argument for the data structure you've figured out

        for (int y = height; y > 0; y--) {
            for (int x = 0; x < width; x++) {
                Vector3 tilePosition = new Vector3(x * tileSize, y * tileSize, 0) + gridOrigin;

                // make a flat cube as a "tile"
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.SetParent(this.transform);
                tile.transform.position = tilePosition;

                counter ++;

                tile.name = counter.ToString();

                TileData info = tile.GetComponent<TileData>();
                info.data = tiles.rooms[counter];
            }
        }

        Vector3 cameraTarget = new Vector3( (width * tileSize/2) - (tileSize/2),
                                            (height * tileSize/2) + (tileSize/2),
                                            -10 );

                Camera.main.transform.position = cameraTarget;
                Camera.main.GetComponent<CameraFollow>().target = cameraTarget;

    }

}