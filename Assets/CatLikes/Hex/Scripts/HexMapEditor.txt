using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexMapEditor : MonoBehaviour
{
    public ToggleGroup group;
    public Color[] colors;
    public HexGrid hexGrid;
    public Slider slider;
    private Color activeColor;
    int activeElevation = 1;

    void Awake(){
        SelectColor(1);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {   int colorIndex = Random.Range(0, colors.Length);
            //group.ActiveToggles
            SelectColor(colorIndex);
            SetElevation(slider.value);
            HandleInput();
        }

    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            HexCell cell = hexGrid.GetCell(hit.point);
            EditCell(cell);
        }
    }

    public void SelectColor(int index){
        activeColor = colors[index];
    }

    public void SetElevation(float elevation) {
        this.activeElevation = (int)elevation;
    }

    void EditCell(HexCell cell) {
        cell.color = activeColor;
        cell.Elevation = activeElevation;
        hexGrid.Refresh();
    }
}
