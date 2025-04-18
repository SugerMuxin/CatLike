using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum OptionalToggle { 
    Ignore,Yes,No
}

public class HexMapEditor : MonoBehaviour
{
    public ToggleGroup group;
    public Toggle showToggle;
    public Toggle elevationToggle;
    public OptionalToggle riverMode;
    public Color[] colors;
    public HexGrid hexGrid;
    public Slider slider,brushSlider;
    private Color activeColor;
    int activeElevation = 1;
    bool applyColor;
    bool applyElevation = true;
    int brushSize;

    void Awake(){
        SelectColor(1);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            ShowUI(showToggle.isOn);
            SetApplyElevation(elevationToggle.isOn);
            SetBrushSize(brushSlider.value);
            SelectColor(GetActiveColorIndex());
            SetElevation(slider.value);
            HandleInput();
            //SetRiverMode(riverMode);
        }
    }

    int GetActiveColorIndex()
    {
        // 方法1：使用 GetFirstActiveToggle（推荐）
        Toggle activeToggle = group.GetFirstActiveToggle();
        if (activeToggle != null)
        {
            int index = activeToggle.transform.GetSiblingIndex();
            return Mathf.Clamp(index, 0, colors.Length - 1);
        }

        // 方法2：使用 Linq 的 FirstOrDefault（需要 using System.Linq）
        // Toggle activeToggle = group.ActiveToggles().FirstOrDefault();
        // if (activeToggle != null)
        // {
        //     int index = activeToggle.transform.GetSiblingIndex();
        //     return Mathf.Clamp(index, 0, colors.Length - 1);
        // }

        return 0; // 默认值
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            HexCell cell = hexGrid.GetCell(hit.point);
            //EditCell(cell);
            EditCells(cell);
        }
    }

    public void SelectColor(int index){
        applyColor = index >= 0;
        if (applyColor) {
            activeColor = colors[index];
        }
    }

    public void SetElevation(float elevation) {
        this.activeElevation = (int)elevation;
    }

    public void SetApplyElevation(bool toogle) { 
        applyElevation = toogle;
    }


    public void SetBrushSize(float size)
    {
        this.brushSize = (int)size;
    }


    void EditCells(HexCell center) {
        int centerX = center.coordinates.X;
        int centerZ = center.coordinates.Z;
        for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
        {
            for (int x = centerX - r; x <= centerX + brushSize; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }
        for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
        {
            for (int x = centerX - brushSize; x <= centerX + r; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }
    }

    void EditCell(HexCell cell) {
        if (cell) {
            if (applyColor)
            {
                cell.color = activeColor;
            }
            if (applyElevation)
            {
                cell.Elevation = activeElevation;
            }
            cell.Refresh();
        }
    }

    public void ShowUI(bool visible)
    {
        hexGrid.ShowUI(visible);
    }

    public void SetRiverMode(int mode)
    {
        riverMode = (OptionalToggle)mode;
    }


}
