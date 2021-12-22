using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [Header("Camera Follow")]
    [SerializeField] private float followSpeed = 0.5f;
    public Vector3 target = new Vector2(0, 0);
    
    [Header("Zoom")]
    [SerializeField] [Range(0f, 5f)] private float zoomSpeedModifier = 1.5f;
    [SerializeField] private float zoomMin = 10;
    [SerializeField] private float zoomMax = 25;

    private Camera cam;

    void Awake() {
        if (!cam) cam = GetComponent<Camera>();    
    }

    void Update() {
        // scroll to zoom
        if (Input.mouseScrollDelta.y != 0) {
            cam.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeedModifier;
        }

        if (cam.orthographicSize > zoomMax) cam.orthographicSize = zoomMax;
        if (cam.orthographicSize < zoomMin) cam.orthographicSize = zoomMin;

    }

    private void LateUpdate() {
        if (transform.position != target) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, -10), followSpeed * Time.deltaTime);
        }
    }

}
