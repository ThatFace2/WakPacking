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

        // draw outside grid
        Gizmos.color = gridSystem.GridColor;
        Gizmos.DrawLineStrip(new Vector3[]
        {
            origin + new Vector3(0, size.y),
            origin + new Vector3(size.z, size.y + -.5f * size.z),
            origin + new Vector3(size.z, -.5f * size.z),
            origin + new Vector3(-size.x + size.z, -.5f * (size.x + size.z)),
            origin + new Vector3(-size.x, -.5f * size.x),
            origin + new Vector3(-size.x, size.y + -.5f * size.x)
        }, true);
        Gizmos.DrawLineList(new Vector3[]
        {
            origin, origin + new Vector3(0, size.y),
            origin, origin + new Vector3(size.z, -.5f * size.z),
            origin, origin + new Vector3(-size.x, -.5f * size.x)
        });

        // draw inside grid
        // 내부 그리드 선은 scene view의 사이즈가 클수록 옅게 조정됩니다
        float min = 2.4f, max = 24f;
        var gridColor = gridSystem.GridColor;
        gridColor.a = 1 - Mathf.LinearToGammaSpace((sceneView.size - min) / (max - min));
        Gizmos.color = gridColor;
        for (var i = 1; i < size.x; i++)
        {
            Gizmos.DrawLine(
                origin + new Vector3(-i, -.5f * i),
                origin + new Vector3(-i, size.y - .5f * i));
            Gizmos.DrawLine(
                origin + new Vector3(-i, -.5f * i),
                origin + new Vector3(size.z - i, -.5f * (size.z + i)));
        }

        for (var i = 1; i < size.y; i++)
        {
            Gizmos.DrawLine(
                origin + new Vector3(0, i),
                origin + new Vector3(-size.x, -.5f * size.x + i));
            Gizmos.DrawLine(
                origin + new Vector3(0, i),
                origin + new Vector3(size.z, -.5f * size.z + i));
        }

        for (var i = 1; i < size.z; i++)
        {
            Gizmos.DrawLine(
                origin + new Vector3(i, -.5f * i),
                origin + new Vector3(i, size.y - .5f * i));
            Gizmos.DrawLine(
                origin + new Vector3(i, -.5f * i),
                origin + new Vector3(-size.x + i, -.5f * (size.x + i)));
        }
    }

    #endregion
}