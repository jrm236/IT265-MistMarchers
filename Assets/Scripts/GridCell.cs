using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int gridPosition;
    public bool isWalkable = true;
    public UnitToken occupyingUnit;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }

    }

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

    public void GridHighlight(bool highlighted)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (highlighted)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = defaultColor;
        }
    }
}
