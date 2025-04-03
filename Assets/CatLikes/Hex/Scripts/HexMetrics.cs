using UnityEngine;

public static class HexMetrics 
{
    public const int chunkSizeX = 5, chunkSizeZ = 5;

    public const float solidFactor = 0.75f;

    public const float blendFactor = 1.0f - solidFactor;

    public const float outerRadius = 10f;

    public const float innerRadius = outerRadius * 0.866025404f;

    public const float elevationStep = 1f;

    public const int terracesPerSlope = 2; //ÿ��б����ƽ̨������//

    public const int terraceSteps = terracesPerSlope * 2 + 1;//���������(���������ƽ�¾����������)//

    public const float horizontalTerraceStepSize = 1f / terraceSteps;//���������ռ����//

    public const float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);//���������ռ����//

    public static Texture2D noiseSource;

    public const float cellPerturbStrength = 3f;

    public const float noiseScale = 0.003f; //���������Ϊ����ÿ��Cell���������ƥ�䵽UV���Ӷ���������Ӱ������//

    //       ����&��
    //    ����   ����
    //    ����   ����
    //       ����
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
    /// ����� v1,v2 �� v3��v3�м�Ĵ�ֱ����//
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetBridge(HexDirection direction) {
        /*���ַ�ʽ���������ڵ�������֮��ʹ��������������*/
        //return (corners[(int)direction] + corners[(int)direction + 1]) * 0.5f * blendFactor;
        /*�������ڵ�������֮����������κϳ�Ϊһ�������Լ���һ�������εĻ���*/
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
