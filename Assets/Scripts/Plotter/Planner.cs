using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour {

    [Header("JSON data")]
    [SerializeField] private TextAsset roomsJSON;
    public RoomList allRooms;

    [Header("Controllables")]
    public int gridSize;

    [Header("Grid")]
    [SerializeField] private int startingGridSize = 123;
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject gridLinePrefab;
    [Space]
    [SerializeField] private List<GameObject> gridLines;

    [Header("Rooms")]
    [SerializeField] private int roomsToPlot;
    [SerializeField] private Transform roomParent;
    [SerializeField] private GameObject roomPrefab;
    [Space]
    [SerializeField] private List<GameObject> rooms;

    [Header("Object References")]
    [SerializeField] private LineRenderer xAxis;
    [SerializeField] private LineRenderer yAxis;

    private void Start() {
        
        gridSize = startingGridSize;

        allRooms = JsonUtility.FromJson<RoomList>(roomsJSON.text);
        // NB access via allRooms.rooms[0].id etc

        // roomsToPlot = allRooms.rooms.Length;
        Debug.Log($"Plotting {roomsToPlot} rooms at random...");

        SetSize(gridSize);
        DrawRegions(gridSize);
        PlotRooms(roomsToPlot);
        
    }

    public void SetSize(float size) {

        // round up to next 10 to create complete 10x10 "regions"
        int roundedSize = (int)Mathf.Ceil(size/10) * 10;

        xAxis.SetPosition(0, Vector2.zero);
        xAxis.SetPosition(1, Vector2.zero + new Vector2(roundedSize, 0f));
        
        yAxis.SetPosition(0, Vector2.zero);
        yAxis.SetPosition(1, Vector2.zero - new Vector2(0f, roundedSize));

        Camera.main.transform.position = new Vector3((roundedSize/6) * 5, -roundedSize/2, -10);
        Camera.main.orthographicSize = roundedSize * 0.6f;

        gridSize = roundedSize;

        // delete additional lines not needed as well
        DrawRegions(roundedSize);
    }

    public void DrawRegions(int newSize) {
        gridSize = newSize;

        for (int x = 10; x <= gridSize; x += 10) {
            DrawGridLine(new Vector2(x, 0), new Vector2(x, -newSize));
        }

        for (int y = 10; y <= gridSize; y += 10) {
            DrawGridLine(new Vector2(0, -y), new Vector2(newSize, -y));
        }

    }

    private void DrawGridLine(Vector2 start, Vector2 end) {
        GameObject obj = Instantiate(gridLinePrefab);
        LineRenderer gridLine = obj.GetComponent<LineRenderer>();

        gridLine.SetPosition(0, start);
        gridLine.SetPosition(1, end);

        obj.name = $"Grid Line ({start} - {end})";
        obj.transform.SetParent(gridParent);
        
        gridLines.Add(obj);
    }

    public void PlotRooms(int number) {

        if (number > Mathf.Pow(gridSize, 2)) {
            gridSize = (int)Mathf.Ceil(Mathf.Sqrt(number)/10) * 10;
        }

        for (int i = 0; i < number; i++) {
            GameObject obj = Instantiate(roomPrefab);
            
            Vector2 _pos = FindEmptyPosition();
            
            obj.name = $"Room ({(int)_pos.x}, {(int)_pos.y})";
            obj.transform.position = _pos;
            obj.transform.SetParent(roomParent);

            rooms.Add(obj);
        }
    }

    public void Replot() {

        foreach (GameObject line in gridLines) GameObject.Destroy(line);
        gridLines.Clear();

        foreach (GameObject room in rooms) GameObject.Destroy(room);
        rooms.Clear();

        SetSize(gridSize);
        PlotRooms(roomsToPlot);

    }

    private Vector2 FindEmptyPosition() {

        Vector2 _randomPosition = new Vector2 ( (int)Mathf.Floor(Random.Range(0f, gridSize)),
                                                -(int)Mathf.Floor(Random.Range(0f, gridSize))
                                              );

        foreach (GameObject room in rooms) {
            if (room.transform.position.x == _randomPosition.x && room.transform.position.y == _randomPosition.x) {
                Debug.Log($"Collision at {_randomPosition}. Finding new position...");
                FindEmptyPosition();
            }
        }

        return _randomPosition;

        // new Vector2(  (int)Mathf.Floor(Random.Range(0f, gridSize)),
        //                                 -(int)Mathf.Floor(Random.Range(0f, gridSize))
        //                               );
    }

}
