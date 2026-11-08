using UnityEngine;
using System.Collections.Generic;

public class UnitToken : MonoBehaviour
{
    [Header("Unit Data")]
    public UnitData unitData;

    [Header("Runtime Data")]
    public int currentHealth;

    [Header("Grid Placement Data")]
    public GridManager gridManager;
    public GridCell currentCell;

    [Header("Visual Placement Data")]
    public Vector3 visualOffset = new Vector3(0f, 0.2f, 0f);

    private Vector3 startingPosition;
    private List<GridCell> reachableCells = new List<GridCell>();
    private Camera mainCamera;
    private bool isDragging = false;

    private void Start()
    {
        mainCamera = Camera.main;

        if (unitData == null) 
        {
            Debug.LogError(gameObject.name + "Unit data is not assigned.");
            return;
        }

        if (gridManager == null)
        {
            Debug.LogWarning(gameObject.name + " has no GridManager assigned.");
            return;
        }
        
        currentHealth = unitData.maxHealth;
        SnapToStartingCell();

        Debug.Log("Unit Loaded: " + unitData.unitName);
        Debug.Log("HP: " + currentHealth);
        Debug.Log("Move: " + unitData.movementSpeed);
        Debug.Log("Armor: " + unitData.armorValue);
        Debug.Log("Grid Position: " + currentCell.gridPosition);
    }

    private void SnapToStartingCell()
    {
        currentCell = gridManager.GetCellFromWorld(transform.position);

        if (currentCell == null)
        {
            Debug.LogWarning(gameObject.name + " is not in a valid grid.");
            return;
        }

        currentCell.occupyingUnit = this;

        transform.position = currentCell.transform.position + visualOffset;

        Debug.Log("Grid Position: " + currentCell.gridPosition);
    }

    private void OnMouseDown()
    {
        if (unitData == null || currentCell == null)
        {
            return;
        }

        isDragging = true;

        startingPosition = transform.position;
        reachableCells = gridManager.GetReachableCells(currentCell, unitData.movementSpeed, this);
    
        foreach (GridCell cell in reachableCells)
        {
            if (cell != currentCell)
            {
                cell.GridHighlight(true);
            }
        }
    }

    private void OnMouseDrag()
    {
        if (!isDragging)
        {
            return;
        }

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = 0;
        transform.position = mousePosition;
    }

    private void OnMouseUp()
    {
        if (!isDragging)
        {
            return;
        }

        isDragging = false;

        GridCell targetCell = gridManager.GetCellFromWorld(transform.position - visualOffset);

        if (targetCell != null && reachableCells.Contains(targetCell))
        {
            MoveToCell(targetCell);
        }
        else
        {
            transform.position = startingPosition;
        }

        foreach (GridCell cell in reachableCells)
        {
            cell.GridHighlight(false);
        }

        reachableCells.Clear();
    }

    private void MoveToCell(GridCell newCell)
    {
        if (currentCell != null)
        {
            currentCell.occupyingUnit = null;
        }

        currentCell = newCell;
        currentCell.occupyingUnit = this;

        transform.position = currentCell.transform.position + visualOffset;

        Debug.Log("Grid Position: " + currentCell.gridPosition);
    }
}
