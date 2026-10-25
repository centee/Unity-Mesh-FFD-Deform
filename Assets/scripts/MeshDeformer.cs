using UnityEngine;
using UnityEngine.UI;

public class MeshDeformer : MonoBehaviour
{
    public MeshFilter mf;
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3 bboxMin, bboxMax;

    void Start()
    {
        mesh = mf.mesh;
        mesh.MarkDynamic();
        originalVertices = mesh.vertices;
        CalcBBox();
        ResetMesh();
    }

    void CalcBBox()
    {
        bboxMin = originalVertices[0];
        bboxMax = originalVertices[0];
        foreach (var p in originalVertices)
        {
            bboxMin = Vector3.Min(bboxMin, p);
            bboxMax = Vector3.Max(bboxMax, p);
        }
    }

    // 重置回原始模型
    public void ResetMesh()
    {
        mesh.vertices = originalVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // 锥化
    public void TaperDeform()
    {
        ResetMesh();
        Vector3[] v = mesh.vertices;
        float topScale = 0.4f;
        for (int i = 0; i < v.Length; i++)
        {
            float t = Mathf.InverseLerp(bboxMin.y, bboxMax.y, v[i].y);
            float s = Mathf.Lerp(1, topScale, t);
            v[i].x *= s;
            v[i].z *= s;
        }
        mesh.vertices = v;
        mesh.RecalculateNormals();
    }

    // 扭曲
    public void TwistDeform()
    {
        ResetMesh();
        Vector3[] v = mesh.vertices;
        float twistStrength = 120f;
        for (int i = 0; i < v.Length; i++)
        {
            float t = Mathf.InverseLerp(bboxMin.y, bboxMax.y, v[i].y);
            float angle = t * twistStrength * Mathf.Deg2Rad;
            float cx = v[i].x, cz = v[i].z;
            v[i].x = cx * Mathf.Cos(angle) - cz * Mathf.Sin(angle);
            v[i].z = cx * Mathf.Sin(angle) + cz * Mathf.Cos(angle);
        }
        mesh.vertices = v;
        mesh.RecalculateNormals();
    }

    // FFD自由变形（替换成直观鼓包版本，函数名不变！按钮不用重新绑定）
    public void FFDDeform()
    {
        ResetMesh();
        Vector3[] v = mesh.vertices;
        float centerY = (bboxMin.y + bboxMax.y) / 2f;
        float heightRange = bboxMax.y - bboxMin.y;

        for (int i = 0; i < v.Length; i++)
        {
            float distY = Mathf.Abs(v[i].y - centerY);
            float factor = 1 - distY / (heightRange / 2f);
            factor = Mathf.Clamp01(factor);
            // 中间向外鼓，幅度很大
            float bulge = factor * factor * 0.7f;
            v[i].x *= (1 + bulge);
            v[i].z *= (1 + bulge);
        }
        mesh.vertices = v;
        mesh.RecalculateNormals();
    }
}

