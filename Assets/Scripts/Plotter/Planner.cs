using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour {

    [Header("JSON data")]
    [SerializeField] private TextAsset roomsJSON;
    public RoomList allRooms;

    [Header("Controllables")]
    public int gridSize = 200;
    public int minimumSpacing = 1;
    public float drawTime = 0.05f;

    [Header("Grid")]
    [SerializeField] private int startingGridSize = 123;
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject gridLinePrefab;
    [SerializeField] private List<GameObject> gridLines;

    [Header("Rooms")]
    public int roomsToPlot;
    [SerializeField] private Transform roomParent;
    [SerializeField] private GameObject roomPrefab;
    [Space]
    [SerializeField] private List<GameObject> rooms;
    [SerializeField] private List<Vector2> occupiedSpaces;

    [Header("Object References")]
    [SerializeField] private LineRenderer xAxis;
    [SerializeField] private LineRenderer yAxis;
    [Space]
    [SerializeField] private CanvasController canvas;

    private void Start() {

        allRooms = JsonUtility.FromJson<RoomList>(roomsJSON.text);
        // NB access via allRooms.rooms[0].id etc

        roomsToPlot = allRooms.rooms.Length;
        Debug.Log($"Plotting {roomsToPlot} rooms at random...");

        // make it so that there's a spacing of at least 1 square between rooms
        SetMinimumSpacing(Mathf.CeilToInt(Mathf.Clamp(minimumSpacing, 1.0f, Mathf.Infinity)));
        SetSize(Mathf.Sqrt(roomsToPlot));

        // twice as big to give us some breathing room
        DrawGrid(gridSize * 2);

        // set the minimum value on the slider to the absolute minimum grid size for the number of rooms with the spacing variable
        canvas.SetSliderRange(gridSize, gridSize * 10);

    }

    public void SetMinimumSpacing(int spacing) {
        minimumSpacing = spacing + 1;
    }

    public void SetSize(float size) {
        if (size == gridSize) return;
        StopAllCoroutines();

        gridSize = RoundedSize(size);
    }

    public void DrawGrid(int size) {
        // draw axes
        xAxis.SetPosition(0, Vector2.zero);
        xAxis.SetPosition(1, Vector2.zero + new Vector2(size, 0f));
        
        yAxis.SetPosition(0, Vector2.zero);
        yAxis.SetPosition(1, Vector2.zero - new Vector2(0f, size));

        // draw region boundaries for ease of use
        for (int x = 10; x <= size; x += 10) {
            DrawGridLine(new Vector2(x, 0), new Vector2(x, -size));
        }

        for (int y = 10; y <= size; y += 10) {
            DrawGridLine(new Vector2(0, -y), new Vector2(size, -y));
        }

        // set camera position to default
        Camera.main.transform.position = new Vector3((size/6) * 5, -size/2, -10);
        Camera.main.orthographicSize = size * 0.6f;

        StartCoroutine(PlotRooms(roomsToPlot));
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

    public IEnumerator PlotRooms(int number) {
        
        int i = 0;
        Vector2 _maxPosition = new Vector2(gridSize, gridSize);

        while (i < number) {
            GameObject obj = Instantiate(roomPrefab);
            
            Vector2 _pos = FindEmptyPosition(Vector2.zero, _maxPosition);
            
            obj.name = $"Room ({(int)_pos.x}, {(int)_pos.y})";
            obj.transform.position = _pos;
            obj.transform.SetParent(roomParent);

            rooms.Add(obj);
            occupiedSpaces.Add(new Vector2(obj.transform.position.x, obj.transform.position.y));

            i++;
            yield return new WaitForSeconds(drawTime);
        }

        Debug.Log("PLOTTED");

    }

    public void Replot() {

        StopAllCoroutines();

        foreach (GameObject line in gridLines) GameObject.Destroy(line);
        gridLines.Clear();

        foreach (GameObject room in rooms) GameObject.Destroy(room);
        rooms.Clear();

        occupiedSpaces.Clear();

        SetSize(gridSize);
        DrawGrid(gridSize);

        StartCoroutine(PlotRooms(roomsToPlot));

    }

    private Vector2 FindEmptyPosition(Vector2 min, Vector2 max) {

        int _attempts = 1;

        Vector2 _randomPosition = new Vector2 ( (int)Mathf.Floor(Random.Range(min.x + minimumSpacing, max.x - minimumSpacing)),
                                                -(int)Mathf.Floor(Random.Range(min.x + minimumSpacing, max.y - minimumSpacing))
                                              );

        // TODO if this has to loop recursively more than a few times (more likely with higher numbers of rooms and larger minimum spacing) Unity will trigger a StackOverFlowException
        foreach (Vector2 occupiedSpace in occupiedSpaces) {
            if (Vector2.Distance(occupiedSpace, _randomPosition) < minimumSpacing) {
                _attempts++;

                // string _msg;

                Random.InitState(Mathf.FloorToInt(Time.time * 1000));
                
                if (Random.Range(0f, 1f) > 0.5f) {
                    FindEmptyPosition(  new Vector2(min.x + minimumSpacing, min.y + minimumSpacing),
                                        new Vector2(_randomPosition.x - minimumSpacing, _randomPosition.y - minimumSpacing));
                    // _msg = " Going higher.";
                } else {
                    FindEmptyPosition(  new Vector2(_randomPosition.x + minimumSpacing, _randomPosition.y + minimumSpacing),
                                        new Vector2(max.x - minimumSpacing, max.y - minimumSpacing));
                    // _msg = " Going lower.";
                }

                // Debug.Log($"Collision at {_randomPosition} (distance of {Vector2.Distance(occupiedSpace, _randomPosition)}). Finding new position (attempt #{_attempts}). {_msg}");
            }
        }

        return _randomPosition;
    }

    private int RoundedSize(float size) {
        
        if (minimumSpacing > 1) {
            size *= minimumSpacing;
            size += minimumSpacing * 2;
        }

        // round up to next 10 to create complete 10x10 "regions"
        return Mathf.CeilToInt(size/10) * 10;

    }


}
