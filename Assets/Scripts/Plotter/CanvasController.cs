using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    [Header("Grid Size")]
    [SerializeField] private Text gridSizeText;
    [SerializeField] private Slider gridSlider;

    [Header("Object References")]
    [SerializeField] private Planner planner;

    private void Update() {
        gridSizeText.text = $"Grid size: {planner.gridSize} x {planner.gridSize}\n{Mathf.Pow(planner.gridSize, 2)} tiles";
    }

    public void SetSliderRange(int min, int max) {
        gridSlider.minValue = min;
        gridSlider.maxValue = max;
    }

    public void SetSliderValue(int value) {
        gridSlider.value = value;
    }

}
