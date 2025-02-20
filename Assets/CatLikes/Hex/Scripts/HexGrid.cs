using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexGrid : MonoBehaviour
{

    public int width = 6;
    public int height = 6;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    public HexCell cellPrefab;
    public TMP_Text cellLabelPrefab;

    Canvas gridCanvas;
    HexMesh hexMesh;
    MeshCollider meshCollider;

    HexCell[] cells;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        cells = new HexCell[width * height];
        int i = 0;
        for (int z= 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x,z,i++);
            }
        }
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    public HexCell GetCell(Vector3 position) { 
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        return cells[index];
    }

    public void Refresh() {
        hexMesh.Triangulate(cells);
    }


    void CreateCell(int x,int z,int i) {
        Vector3 position;

        //四边形的空间排版//
        //position.x = x * 10f;
        //position.y = 0f;
        //position.z = z * 10f;

        //六边形的空间排版//
        position.x = (x + z*0.5f - z/2) * HexMetrics.innerRadius * 2;
        position.y = 0;
        position.z = z * HexMetrics.outerRadius * 1.5f;

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform,false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x,z);
        cell.color = defaultColor;
        if (x > 0) {
            cell.SetNeighbor(HexDirection.W, cells[i-1]);  //西边的邻居and东边的邻居//
        }
        if (z > 0) {
            if ((z & 1) == 0)
            {   //偶数行再做判断//
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1) {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        TMP_Text label = Instantiate<TMP_Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeprateLines();

        cell.uiRect = label.rectTransform;
    }


}
