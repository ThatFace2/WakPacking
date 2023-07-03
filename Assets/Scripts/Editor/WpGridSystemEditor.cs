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

        Gizmos.color = gridSystem.GridColor;
        Gizmos.DrawLineStrip(new Vector3[]
        {
            new(0, size.y),
            new(size.z, size.y + -.5f * size.z),
            new(size.z, -.5f * size.z),
            new(-size.x + size.z, -.5f * (size.x + size.z)),
            new(-size.x, -.5f * size.x),
            new(-size.x, size.y + -.5f * size.x)
        }, true);
        Gizmos.DrawLineList(new Vector3[]
        {
            Vector3.zero,
            new(0, size.y),
            Vector3.zero,
            new(size.z, -.5f * size.z),
            Vector3.zero,
            new(-size.x, -.5f * size.x)
        });

        float min = 2.4f, max = 24f;
        var gridColor = gridSystem.GridColor;
        gridColor.a = 1 - Mathf.LinearToGammaSpace((sceneView.size - min) / (max - min));
        Gizmos.color = gridColor;
        for (var i = 1; i < size.x; i++)
        {
            Gizmos.DrawLine(
                new Vector3(-i, -.5f * i),
                new Vector3(-i, size.y - .5f * i));
            Gizmos.DrawLine(
                new Vector3(-i, -.5f * i),
                new Vector3(size.z - i, -.5f * (size.z + i)));
        }

        for (var i = 1; i < size.y; i++)
        {
            Gizmos.DrawLine(
                new Vector3(0, i),
                new Vector3(-size.x, -.5f * size.x + i));
            Gizmos.DrawLine(
                new Vector3(0, i),
                new Vector3(size.z, -.5f * size.z + i));
        }

        for (var i = 1; i < size.z; i++)
        {
            Gizmos.DrawLine(
                new Vector3(i, -.5f * i),
                new Vector3(i, size.y - .5f * i));
            Gizmos.DrawLine(
                new Vector3(i, -.5f * i),
                new Vector3(-size.x + i, -.5f * (size.x + i)));
        }
    }

    #endregion
}