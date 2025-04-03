using UnityEngine;

public static class HexMetrics 
{
    public const int chunkSizeX = 5, chunkSizeZ = 5;

    public const float solidFactor = 0.75f;

    public const float blendFactor = 1.0f - solidFactor;

    public const float outerRadius = 10f;

    public const float innerRadius = outerRadius * 0.866025404f;

    public const float elevationStep = 1f;

    public const int terracesPerSlope = 2; //每个斜坡中平台的数量//

    public const int terraceSteps = terracesPerSlope * 2 + 1;//阶面的数量(如果是两个平坡就有五个阶面)//

    public const float horizontalTerraceStepSize = 1f / terraceSteps;//横向阶面所占比例//

    public const float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);//纵向阶面所占比例//

    public static Texture2D noiseSource;

    public const float cellPerturbStrength = 3f;

    public const float noiseScale = 0.003f; //这个参数是为了让每个Cell传入的坐标匹配到UV，从而让噪声的影响连贯//

    //       ·①&⑥
    //    ·⑥   ·②
    //    ·⑤   ·③
    //       ·④
    public static Vector3[] corners = {
        new Vector3(0,0,outerRadius),
        new Vector3(innerRadius,0,0.5f*outerRadius),
        new Vector3(innerRadius,0,-0.5f*outerRadius),
        new Vector3(0,0,-outerRadius),
        new Vector3(-innerRadius,0,-0.5f*outerRadius),
        new Vector3(-innerRadius,0,0.5f*outerRadius),
        new Vector3(0,0,outerRadius)
    };

    public static Vector3 GetFirstCorner(HexDirection direction) { 
        return corners[(int)direction];
    }

    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int)direction + 1];
    }

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return corners[(int)direction] * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction) {
        return corners[(int)direction + 1] * solidFactor;
    }

    /// <summary>
    /// 计算出 v1,v2 与 v3，v3中间的垂直向量//
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetBridge(HexDirection direction) {
        /*这种方式在两个相邻的六边形之间使用两个矩形连接*/
        //return (corners[(int)direction] + corners[(int)direction + 1]) * 0.5f * blendFactor;
        /*两个相邻的六边形之间的两个矩形合成为一个，可以减少一个三角形的绘制*/
        return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
    }

    public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step) {
        float h = step * HexMetrics.horizontalTerraceStepSize;
        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;
        float v = ((step + 1) / 2) * HexMetrics.verticalTerraceStepSize;
        a.y += (b.y - a.y) * v;
        return a;
    }

    public static Color TerraceLerp(Color a, Color b, int step) {
        float h = step * HexMetrics.horizontalTerraceStepSize;
        return Color.Lerp(a,b,h); 
    }

    public static HexEdgeType GetEdgeType(int elevation1, int elevation2) {
        if (elevation1 == elevation2) {
            return HexEdgeType.Flat;
        }
        int delta = elevation2 - elevation1;
        if (delta == 1 || delta == -1) {
            return HexEdgeType.Slope;
        }
        return HexEdgeType.Cliff;
    }

    public static Vector4 SampleNoise(Vector3 position) {
        return noiseSource.GetPixelBilinear(position.x * noiseScale, position.z * noiseScale);
    }

}
