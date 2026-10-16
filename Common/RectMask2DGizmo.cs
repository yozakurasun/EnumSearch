using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class RectMask2DGizmo : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        var mask = GetComponent<RectMask2D>();
        var rect = GetComponent<RectTransform>();

        if (mask == null || rect == null) return;

        var r = rect.rect;

        // Padding適用後の範囲
        float left = r.xMin + mask.padding.x;
        float right = r.xMax - mask.padding.z;
        float bottom = r.yMin + mask.padding.y;
        float top = r.yMax - mask.padding.w;

        Vector3[] corners = new Vector3[4];
        corners[0] = rect.TransformPoint(new Vector3(left, bottom));
        corners[1] = rect.TransformPoint(new Vector3(left, top));
        corners[2] = rect.TransformPoint(new Vector3(right, top));
        corners[3] = rect.TransformPoint(new Vector3(right, bottom));

        Gizmos.color = Color.green;

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
        }
    }
}