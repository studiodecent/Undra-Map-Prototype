using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] [Range(0f, 20f)] private float moveSpeed = 12f;
    [SerializeField] [Range(0f, 10f)] private float scrollSpeed = 5f;
    [Space]
    [SerializeField] [Range(1f, 10f)] private float maxZoom = 8.5f;
    [SerializeField] [Range(50f, 100f)] private float minZoom = 75f;

    private Camera cam;
    private Vector3 mousePosition;
    private Vector3 cameraTarget;

    private void Awake() {
        if (cam == null) cam = this.GetComponent<Camera>();
    }

    private void Update() {

        // move camera with WASD/arrow keys
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            cameraTarget += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f) * moveSpeed * Time.deltaTime;
        }

        // get the mouse position to centre the camera on that point
        // (unless that point is on the canvas, currently hardcoded to the last 30% of screen space)
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.mousePosition.x < cam.pixelWidth * 0.7f) {
            
            if (Input.GetMouseButtonDown(1)) {
                cameraTarget = new Vector3(mousePosition.x, mousePosition.y, -10);
            }

            if (Input.mouseScrollDelta.y != 0f) {
                // zoom with the scroll wheel
                cam.orthographicSize -= Input.mouseScrollDelta.y * scrollSpeed;
                
                if (cam.orthographicSize < maxZoom) cam.orthographicSize = maxZoom;
                if (cam.orthographicSize > minZoom) cam.orthographicSize = minZoom;
            }
        }

        cam.transform.position = Vector3.Lerp(cam.transform.position, cameraTarget, moveSpeed * Time.deltaTime);

    }

    public void SetCamera(int gridSize) {
        Vector3 _defaultPosition = new Vector3((gridSize/6) * 5, -gridSize/2, -10);
        cam.transform.position = _defaultPosition;
        cameraTarget = _defaultPosition;

        // set the furthest-out zoom level of the camera to fit the whole thing on-screen
        minZoom = gridSize * 0.6f;
        cam.orthographicSize = minZoom;
    }

}
