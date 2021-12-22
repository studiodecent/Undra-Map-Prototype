using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSelect : MonoBehaviour {

    private MapManager mapManager;

    void Start() {
        mapManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MapManager>();
    }

    void Update() {

        if (Input.GetMouseButtonDown(0)) {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider != null) {
                    int mapToLoad;

                    if (int.TryParse(hit.collider.name, out mapToLoad)) {
                        // check if the mapToLoad isn't trying to load the global view
                        if (mapToLoad >= 0) mapManager.OpenLocalMap(mapToLoad);
        
                    } else {
                        Debug.LogError($"{hit.collider.name} is not a valid map number");
                    }
                }
            }

        }
    }
}
