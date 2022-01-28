using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    [Header("Room Data")]
    [SerializeField] private Text info;

    [Header("Grid Size")]
    [SerializeField] private Text gridSizeText;
    [SerializeField] private Slider gridSlider;

    [Header("Minimum Spacing")]
    [SerializeField] private Text spacingText;

    [Header("Object References")]
    [SerializeField] private Planner planner;

    private void Update() {
        gridSizeText.text = planner.gridSize.ToString();
    }

    public void SetSliderRange(int min, int max) {
        gridSlider.minValue = min;
        gridSlider.maxValue = max;
    }

    public void SetSliderValue() {
        gridSlider.value = gridSlider.minValue;
    }

    public void SetMinimumSpacing(int value) {
        spacingText.text = value.ToString();
    }

    public void DisplayInfo() {
        info.text = "mouse over\na room\nfor details";
    }

    public void DisplayInfo(Room room) {
        info.text = $"{room.id}\n{room.rarity} {room.type}\n{room.owner}";
    }

}
