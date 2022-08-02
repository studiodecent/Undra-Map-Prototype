using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Planner : MonoBehaviour {

    [Space]

    public int gridSize = 200;
    public int minimumSpacing = 1;

    [Header("JSON Data")]
    [SerializeField] private TextAsset roomsJSON;
    public RoomList data;

    [Space]
    [Space]
    [Space]

    [Header("Object References")]
    [SerializeField] private Transform roomParent;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private List<GameObject> roomObjects;
    [Space]
    [SerializeField] private List<Vector2> possibleSpaces;
    [Space]
    [SerializeField] private LineRenderer xAxis;
    [SerializeField] private LineRenderer yAxis;
    [Space]
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject gridLinePrefab;
    [SerializeField] private List<GameObject> gridLines;
    [Space]
    [SerializeField] private CanvasController canvas;
    
    private CameraController cam;
    private int roomsToPlot;
    
    private void Start() {

        cam = Camera.main.GetComponent<CameraController>();

        // NB access via data.rooms[0].id etc
        data = JsonUtility.FromJson<RoomList>(roomsJSON.text);
        roomsToPlot = data.rooms.Length;

        if (minimumSpacing <= 0) minimumSpacing = 1;
        gridSize = GetRoundedSize(GetMinimumSize(roomsToPlot, minimumSpacing));

        canvas.SetSliderRange(gridSize, gridSize * 10);

        Replot();        

    }

    public void Replot() {
        StopAllCoroutines();

        if (gridLines.Count > 0){
            foreach (GameObject line in gridLines) GameObject.Destroy(line);
            gridLines.Clear();
        }

        if (roomObjects.Count > 0) {
            foreach (GameObject room in roomObjects) GameObject.Destroy(room);
            roomObjects.Clear();
        }

        if (possibleSpaces.Count > 0) possibleSpaces.Clear();

        SetSize(gridSize);
        DrawGrid(gridSize);
    }

    public void DrawGrid(int size) {
        // draw axes
        xAxis.SetPosition(0, Vector2.zero);
        xAxis.SetPosition(1, Vector2.zero + new Vector2(size, 0f));
        
        yAxis.SetPosition(0, Vector2.zero);
        yAxis.SetPosition(1, Vector2.zero - new Vector2(0f, size));

        // set slider range: minimum possible size to 10* the minimum possible size
        int _minimumSize = GetMinimumSize(roomsToPlot, minimumSpacing);
        canvas.SetSliderRange(_minimumSize, _minimumSize * 10);

        // draw region boundaries for ease of use
        for (int x = 10; x <= size; x += 10) {
            DrawGridLine(new Vector2(x, 0), new Vector2(x, -size));
        }

        for (int y = 10; y <= size; y += 10) {
            DrawGridLine(new Vector2(0, -y), new Vector2(size, -y));
        }

        cam.SetCamera(size);

        StartCoroutine(PlotRooms(roomsToPlot));
    }

    public void SetMinimumSpacing(int spacing) {
        minimumSpacing = Mathf.CeilToInt(Mathf.Clamp(spacing, 1.0f, Mathf.Infinity));
    }

    public void SetMinimumSpacing(string text) {
        int spacing;

        if (int.TryParse(text, out spacing)) {
            minimumSpacing = Mathf.CeilToInt(Mathf.Clamp(spacing, 1.0f, Mathf.Infinity));
        } else {
            Debug.LogError("That's not a number!");
        }
    }

    public void SetSize(float size) {
        if (size == gridSize) return;
        gridSize = GetRoundedSize(size);
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

        for (int r = 0; r < data.rooms.Length ; r++) {
            GameObject obj = Instantiate(roomPrefab);
            Room room = data.rooms[r];

            obj.transform.SetParent(roomParent);
            obj.GetComponent<RoomPlot>().SetData(room);            
            roomObjects.Add(obj);

            Vector2 pos = FindEmptyPosition();
            obj.transform.position = pos;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ListPossibleSpaces() {
        int _max = gridSize - minimumSpacing;

        for (int x = minimumSpacing; x < _max ; x += minimumSpacing) {
            for (int y = 0 - minimumSpacing; y > 0 - _max; y -= minimumSpacing) {
                possibleSpaces.Add(new Vector2(x, y));
            }
        }

        yield return null;
    }

    private Vector2 FindEmptyPosition() {
        int _random = Mathf.FloorToInt(Random.Range(0, possibleSpaces.Count - 1));

        Vector2 _randomPosition = possibleSpaces[_random];
        possibleSpaces.RemoveAt(_random);

        return _randomPosition;
    }

    private int GetRoundedSize(float size) {
        // round up to next 10 to create complete 10x10 "regions"
        return Mathf.CeilToInt(size/10) * 10;
    }

    private int GetMinimumSize(int rooms, int spacing) {
        return Mathf.CeilToInt(Mathf.Sqrt(rooms) * spacing);
    }


}
