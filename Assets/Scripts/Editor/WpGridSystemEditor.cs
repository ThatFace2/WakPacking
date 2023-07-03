using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WpGridSystem))]
public class WpGridSystemEditor : Editor
{
    #region Private Field

    private static bool? k_CachedSceneViewGridVisibility;

    #endregion

    #region Private Methods

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    private static void DrawGizmos(WpGridSystem gridSystem, GizmoType gizmoType)
    {
        var sceneView = SceneView.currentDrawingSceneView;

        if (sceneView is null)
            return;

        // GridSystem의 Gizmo를 표시하는 동안은 SceneView의 Grid를 비활성화
        if (gizmoType.HasFlag(GizmoType.NonSelected))
        {
            if (k_CachedSceneViewGridVisibility.HasValue)
            {
                sceneView.showGrid = k_CachedSceneViewGridVisibility.Value;
                k_CachedSceneViewGridVisibility = null;
            }

            return;
        }

        if (gizmoType.HasFlag(GizmoType.Selected)
            && !k_CachedSceneViewGridVisibility.HasValue)
        {
            k_CachedSceneViewGridVisibility = sceneView.showGrid;
            sceneView.showGrid = false;
        }

        var size = gridSystem.GridSize;
        var origin = gridSystem.transform.position;
        var yAxis = gridSystem.transform.up;
        var xAxis = -gridSystem.transform.right - 0.5f * gridSystem.transform.up;
        var zAxis = gridSystem.transform.right - 0.5f * gridSystem.transform.up;

        // draw outside grid
        Gizmos.color = gridSystem.GridColor;
        Gizmos.DrawLineStrip(new Vector3[]
        {
            origin + size.y * yAxis,
            origin + size.y * yAxis + size.z * zAxis,
            origin + size.z * zAxis,
            origin + size.x * xAxis + size.z * zAxis,
            origin + size.x * xAxis,
            origin + size.x * xAxis + size.y * yAxis
        }, true);
        Gizmos.DrawLineList(new Vector3[]
        {
            origin, size.x * xAxis,
            origin, size.y * yAxis,
            origin, size.z * zAxis
        });

        // draw inside grid
        // 내부 그리드 선은 scene view의 사이즈가 클수록 옅게 조정됩니다
        float min = 4.8f, max = 48f;
        var gridColor = gridSystem.GridColor;
        gridColor.a = 1 - Mathf.LinearToGammaSpace((sceneView.cameraDistance - min) / (max - min));
        Gizmos.color = gridColor;
        for (var i = 1; i < size.x; i++)
        {
            Gizmos.DrawLine(
                origin + i * xAxis,
                origin + i * xAxis + size.y * yAxis);
            Gizmos.DrawLine(
                origin + i * xAxis,
                origin + i * xAxis + size.z * zAxis);
        }

        for (var i = 1; i < size.y; i++)
        {
            Gizmos.DrawLine(
                origin + i * yAxis,
                origin + i * yAxis + size.x * xAxis);
            Gizmos.DrawLine(
                origin + i * yAxis,
                origin + i * yAxis + size.z * zAxis);
        }

        for (var i = 1; i < size.z; i++)
        {
            Gizmos.DrawLine(
                origin + i * zAxis,
                origin + i * zAxis + size.y * yAxis);
            Gizmos.DrawLine(
                origin + i * zAxis,
                origin + i * zAxis + size.x * xAxis);
        }
    }

    #endregion
}