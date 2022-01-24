using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] private List<Vector2> possibleSpaces;

    [Header("Object References")]
    [SerializeField] private LineRenderer xAxis;
    [SerializeField] private LineRenderer yAxis;
    [Space]
    [SerializeField] private CanvasController canvas;

    private void Start() {

        // NB access via allRooms.rooms[0].id etc
        allRooms = JsonUtility.FromJson<RoomList>(roomsJSON.text);

        roomsToPlot = allRooms.rooms.Length;
        Debug.Log($"Plotting {roomsToPlot} rooms at random...");

        SetMinimumSpacing(minimumSpacing);
        SetSize(Mathf.Sqrt(roomsToPlot));

        canvas.SetSliderRange(Mathf.CeilToInt(Mathf.Sqrt(roomsToPlot)), Mathf.CeilToInt(Mathf.Sqrt(roomsToPlot) * 10));

        DrawGrid(gridSize);

    }

    public void SetMinimumSpacing(int spacing) {
        minimumSpacing = Mathf.CeilToInt(Mathf.Clamp(spacing, 1.0f, Mathf.Infinity));
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

        yield return ListPossibleSpaces();

        int i = 0;

        for (int r = 0; r < allRooms.rooms.Length ; r++) {
            GameObject obj = Instantiate(roomPrefab);
            
            Vector2 _pos = FindEmptyPosition();
            
            obj.name = $"{allRooms.rooms[r].id}";
            obj.transform.position = _pos;
            obj.transform.SetParent(roomParent);

            obj.GetComponent<RoomPlot>().SetData(allRooms.rooms[r]);
            
            rooms.Add(obj);
            // occupiedSpaces.Add(new Vector2(obj.transform.position.x, obj.transform.position.y));

            i++;
            yield return new WaitForSeconds(drawTime);
        }

        Debug.Log("PLOTTED");

    }

    private IEnumerator ListPossibleSpaces() {
        int _max = gridSize - minimumSpacing;

        for (int x = minimumSpacing; x < gridSize ; x += minimumSpacing) {
            for (int y = 0 - minimumSpacing; y > 0 - gridSize; y -= minimumSpacing) {
                possibleSpaces.Add(new Vector2(x, y));
            }
        }

        yield return null;
    }

    public void Replot() {

        StopAllCoroutines();

        foreach (GameObject line in gridLines) GameObject.Destroy(line);
        gridLines.Clear();

        foreach (GameObject room in rooms) GameObject.Destroy(room);
        rooms.Clear();

        possibleSpaces.Clear();

        SetSize(gridSize);
        DrawGrid(gridSize);

    }

    private Vector2 FindEmptyPosition() {
        int _random = Mathf.FloorToInt(Random.Range(0, possibleSpaces.Count - 1));

        Vector2 _randomPosition = possibleSpaces[_random];

        possibleSpaces[_random] = possibleSpaces[possibleSpaces.Count - 1];
        possibleSpaces.RemoveAt(possibleSpaces.Count -1);

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
