using UnityEngine;

public class WpGridSystem : MonoBehaviour
{
    #region Serialize Field

    [SerializeField] private Vector3Int m_GridSize;
    [SerializeField] private Color m_GridColor;

    #endregion


    #region Public Property

    public Vector3Int GridSize => m_GridSize;
    public Color GridColor => m_GridColor;

    #endregion
}