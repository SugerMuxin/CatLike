
using UnityEngine;

[System.Serializable]
public struct HexCoordinates {
    [SerializeField]
    private int x, z;

    public int X { 
        get {
            return x;
        } 
    }

    public int Y { 
        get {
            return -X - Z;
        }
    }

    public int Z {
        get {
            return z;
        }
    }

    public HexCoordinates(int x, int z) { 
        this.x =x; this.z = z;
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);
        if (iX + iY + iZ != -1) {
            Debug.LogWarning("rounding error!");
            /*由于精度的问题造成边界的地方可能计算错误需要矫正*/
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }
        return new HexCoordinates(iX, iZ);
    }

    public static HexCoordinates FromOffsetCoordinates(int x,int z) { 
        return new HexCoordinates(x - z/2,z);
    }

    public override string ToString()
    {
        return $"({X.ToString()},{Y.ToString()},{Z.ToString()})";
    }

    public string ToStringOnSeprateLines() {
        return X.ToString() + "\n" +Y.ToString() +"\n"+ Z.ToString();
    }

}

public enum HexDirection { 
    NE,
    E,
    SE,
    SW,
    W,
    NW
}

public static class HexDirectionExtensions {
    public static HexDirection Opposite(this HexDirection direction) {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    public static HexDirection Previous(this HexDirection direction) {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }

}

public enum HexEdgeType { 
    Flat,
    Slope,
    Cliff
}


public class HexCell : MonoBehaviour
{
    public RectTransform uiRect;

    public HexCoordinates coordinates;

    public Color color;

    int elevation;

    public int Elevation
    {
        get {
            return elevation;
        }
        set {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value* HexMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
        }
    }

    [SerializeField]
    HexCell[] neighbors;

    public HexCell GetNeighbor(HexDirection direction) {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction,HexCell cell) {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public HexEdgeType GetEdgeType(HexDirection direction){
        if (neighbors[(int)direction] != null) {
            return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
        }
        return HexEdgeType.Flat;
    }

    public HexEdgeType GetEdgeType(HexCell otherCell) {
        return HexMetrics.GetEdgeType(elevation,otherCell.elevation);
    }


}
