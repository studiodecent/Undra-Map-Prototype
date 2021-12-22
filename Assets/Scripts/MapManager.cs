using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour {

    [Header("This Map")]
    public bool global = true;
    [Tooltip("The map to load")]
    public int currentMap = -1;
    public int regionSize = 100;

    [Header("Room Data")]
    public TextAsset roomsJSON;

    [Space]
    [SerializeField] private GridSketcher grid;
    [Space]
    [SerializeField] private RoomList allRooms;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() {
        allRooms = JsonUtility.FromJson<RoomList>(roomsJSON.text);
        // NB access via allRooms.rooms[0].id etc
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Local View") {
            grid = GameObject.FindGameObjectWithTag("Local Grid Maker").GetComponent<GridSketcher>();
            CreateMap();
        } else {
            grid = null;
            // data = new Room[0];
        }
    }

    // NB load the scene with a nice coroutine and transition
    public void OpenLocalMap(int region) {
        currentMap = region;
        SceneManager.LoadScene("Scenes/Local View");
    }

    public void OpenGlobalMap() {
        SceneManager.LoadScene("Scenes/Global View");
        currentMap = -1;
    }

    private void CreateMap() {
        grid.tiles = new RoomList(regionSize);

        for (int i = 0; i < regionSize; i++) {
            grid.tiles.rooms[i] = allRooms.rooms[currentMap * regionSize + i];
        }

        grid.CreateGrid(currentMap);

    }

}