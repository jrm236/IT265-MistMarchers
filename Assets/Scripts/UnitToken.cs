using UnityEngine;

public class UnitToken : MonoBehaviour
{
    [Header("Unit Data")]
    public UnitData unitData;

    [Header("Runtime Data")]
    public int currentHealth;

    [Header("Grid Placement Data")]
    public GridManager gridManager;
    public GridCell currentCell;

    private void Start()
    {
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

        transform.position = currentCell.transform.position;
    }
}
