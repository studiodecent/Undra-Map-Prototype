using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSelect : MonoBehaviour {
    
    [Header("Raycast Layer Mask")]
    [SerializeField] private LayerMask layerMask;

    [Header("Information Display")]
    [SerializeField] private GameObject infoPanel;
    [Space]
    [SerializeField] private Text roomNumber;
    [SerializeField] private Text ownerName;

    private GridSketcher grid;
    private MapManager mapManager;
    private CameraFollow cameraFollow;
    private Vector3 clickPos;
    private float zOffset;
    private CameraFollow cam;
    
    private void Start() {
        zOffset = this.transform.position.z;
        cam = Camera.main.GetComponent<CameraFollow>();

        grid = GetComponent<GridSketcher>();
        mapManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
    }

    void Update() {

        // TODO layer mask the UI from the raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        TileData tile;

        if (Physics.Raycast(ray, out hit)) {
            tile = hit.collider.gameObject.GetComponent<TileData>();

            if (tile) {
                infoPanel.SetActive(true);
                roomNumber.text = $"{tile.data.id}";
                ownerName.text = $"{tile.data.owner}";
            } else {
                infoPanel.SetActive(false);
            }

        }

        if (Input.GetMouseButtonDown(0) && hit.collider != null) {
            cam.target = new Vector3 (  hit.collider.GetComponent<Renderer>().bounds.center.x, 
                                        hit.collider.GetComponent<Renderer>().bounds.center.y, 
                                        zOffset);
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) {
            mapManager.OpenGlobalMap();
        }

    }
}
