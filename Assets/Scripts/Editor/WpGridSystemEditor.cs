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
    }

    #endregion
}