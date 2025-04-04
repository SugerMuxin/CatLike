using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexGridChunk : MonoBehaviour
{
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    public HexCell cellPrefab;
    public TMP_Text cellLabelPrefab;
    HexCell[] cells;

    HexMesh hexMesh;
    Canvas gridCanvas;

    int width;
    int height;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        width = HexMetrics.chunkSizeX;
        height = HexMetrics.chunkSizeZ;
        cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        hexMesh.Triangulate(cells);
    }

    public void AddCell(int index, HexCell cell)
    {
        cells[index] = cell;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }

}
