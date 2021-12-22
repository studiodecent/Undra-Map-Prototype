using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    [Header("Grid Size")]
    [SerializeField] private Text gridSizeText;
    [SerializeField] private Slider gridSlider;

    [Header("Object References")]
    [SerializeField] private Planner planner;

    private void Update() {
        
        gridSizeText.text = $"Grid size: {planner.gridSize} x {planner.gridSize}\n{Mathf.Pow(gridSlider.value, 2)} tiles";

    }

}
