using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Size Settings")]
    public int gridWidth = 6;
    public int gridHeight = 6;

    public float cellSize = 1f;

    [Header("Cell Prefab Settings")]
    public GridCell cellPrefab;
    private GridCell[,] cells;
    private Vector3 gridOrigin;

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        cells = new GridCell[gridWidth, gridHeight];
        gridOrigin = transform.position;

        gridOrigin.x -= (gridWidth - 1) * cellSize / 2f;
        gridOrigin.y -= (gridHeight - 1) * cellSize / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPosition = gridOrigin + new Vector3(x * cellSize, y * cellSize, 0);
                GridCell newCell = Instantiate(cellPrefab, worldPosition, Quaternion.identity, transform);

                newCell.Setup(new Vector2Int(x, y));
                cells[x, y] = newCell;
            }
        }
    }

    public GridCell GetCell(Vector2Int position)
    {
        if (position.x < 0 || position.x >= gridWidth || position.y < 0 || position.y >= gridHeight)
        {
            return null;
        }

        return cells[position.x, position.y];
    }

    public GridCell GetCellFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - gridOrigin.x) / cellSize);
        int y = Mathf.RoundToInt((worldPosition.y - gridOrigin.y) / cellSize);

        return GetCell(new Vector2Int(x, y));
    }
}
