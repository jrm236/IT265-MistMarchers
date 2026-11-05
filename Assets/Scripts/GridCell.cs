using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int gridPosition;
    public bool isWalkable = true;
    public UnitToken occupyingUnit;

    public void Setup(Vector2Int position)
    {
        gridPosition = position;
    }

    public bool IsAvailable(UnitToken movingUnit)
    {
        if (!isWalkable)
        {
            return false;
        }

        if (occupyingUnit != null && occupyingUnit != movingUnit)
        {
            return false;
        }

        return true;
    }
}
