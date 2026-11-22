using UnityEngine;
using System.Collections.Generic;

public class UnitToken : MonoBehaviour
{
    [Header("Unit Data")]
    public UnitData unitData;

    [Header("Runtime Data")]
    public int currentHealth;

    [Header("Player Ownership Data")]
    public PlayerID owner;

    [Header("Grid Placement Data")]
    public GridCell currentCell;

    [Header("Visual Placement Data")]
    public Vector3 visualOffset = new Vector3(0f, 0.2f, 0f);

    [Header("Managers")]
    public TurnManager turnManager;
    public GridManager gridManager;
    public UnitSelectionManager unitSelectionManager;

    [Header("Unit Activation Data")]
    public bool activationStarted = false;
    public bool hasMovedThisActivation = false;
    public bool hasAttackedThisActivation = false;

    private Vector3 startingPosition;
    private List<GridCell> reachableCells = new List<GridCell>();
    private List<GridCell> attackableCells = new List<GridCell>();
    private Camera mainCamera;
    private bool isDragging = false;
    private bool movementEnabled = false;
    private bool attackEnabled = false;

    private SpriteRenderer spriteRenderer;
    private Color normalColor;

    private void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        if (spriteRenderer != null)
        {
            normalColor = spriteRenderer.color;
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
        if (unitSelectionManager == null)
        {
            return;
        }

        UnitToken attacker = unitSelectionManager.selectedUnit;

        if (movementEnabled)
        {
            if (unitSelectionManager.selectedUnit != this)
            {
                return;
            }

            isDragging = true;
            startingPosition = transform.position;

            if (attackEnabled)
            {
                CancelUnitAttack();
            }

            return;
        }

        if (attacker != null && unitSelectionManager.currentUnitState == UnitState.UnitAttacking)
        {
            attacker.TryAttack(this);
            return;
        }  

        unitSelectionManager.SelectUnit(this);

    }

    private void OnMouseDrag()
    {
        if (!movementEnabled)
        {
            return;
        }

        if (unitSelectionManager == null)
        {
            Debug.LogError(gameObject.name + " has no UnitSelectionManager assigned.");
            return;
        }

        if (unitSelectionManager.selectedUnit != this)
        {
            return;
        }

        if (!isDragging)
        {
            return;
        }

        if (mainCamera == null)
        {
            return;
        }

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

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

        if (targetCell != null && reachableCells.Contains(targetCell) && targetCell != currentCell)
        {
            if (!activationStarted)
            {
                bool activationConfirmed = unitSelectionManager.ConfirmActivation(this);

                if (!activationConfirmed)
                {
                    transform.position = startingPosition;

                    return;
                }
            }

            MoveToCell(targetCell);

            hasMovedThisActivation = true;
            unitSelectionManager.RefreshUnitUI();
        }
        else
        {
            transform.position = startingPosition;
        }

        ClearMovementHighlights();
        movementEnabled = false;

        if (unitSelectionManager != null)
        {
            unitSelectionManager.currentUnitState = UnitState.UnitSelected;
        }
    }

    private void ClearMovementHighlights()
    {
        foreach (GridCell cell in reachableCells)
        {
            cell.ClearHighlight();
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

    public void SetSelected(bool selected)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (selected)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = normalColor;
        }
    }

    public void BeginUnitMovement()
    {
        if (unitData == null || currentCell == null || turnManager == null)
        {
            return;
        }

        movementEnabled = true;

        reachableCells = gridManager.GetReachableCells(currentCell, unitData.movementSpeed, this);
    
        foreach (GridCell cell in reachableCells)
        {
            if (cell != currentCell)
            {
                cell.GridHighlight(Color.green);
            }
        }
    }

    public void CancelUnitMovement()
    {
        ClearMovementHighlights();
        movementEnabled = false;
        isDragging = false;

        if (currentCell != null)
        {
            transform.position = currentCell.transform.position + visualOffset;
        }
    }

    public void StartActivation()
    {
        activationStarted = true;

        Debug.Log("Unit Activation Started: " + unitData.unitName);
    }

    public void EndActivation()
    {
        activationStarted = false;
        hasMovedThisActivation = false;
        hasAttackedThisActivation = false;

        CancelUnitMovement();
        CancelUnitAttack();

        Debug.Log("Unit Activation Ended: " + unitData.unitName);
    }

    public void BeginUnitAttack()
    {
        if (unitData == null || currentCell == null)
        {
            return;
        }

        attackEnabled = true;
        attackableCells = gridManager.GetAttackableCells(currentCell, unitData.attackRange);

        foreach (GridCell cell in attackableCells)
        {
            cell.GridHighlight(new Color(1f, 0.55f, 0.25f));
        }

        if (movementEnabled)
        {
            CancelUnitMovement();
        }
    }

    public void CancelUnitAttack()
    {
        foreach (GridCell cell in attackableCells)
        {
            cell.ClearHighlight();
        }

        attackableCells.Clear();
        attackEnabled = false;
    }

    public void TryAttack(UnitToken targetUnit)
    {
        if (targetUnit == null)
        {
            return;
        }

        if (!attackEnabled)
        {
            return;
        }

        if (targetUnit.owner == owner)
        {
            Debug.Log("Cannot attack ally unit.");
            return;
        }

        if (!attackableCells.Contains(targetUnit.currentCell))
        {
            Debug.Log("Target unit is out of targetable range.");
            return;
        }

        if (!activationStarted)
        {
            bool activationConfirmed =  unitSelectionManager.ConfirmActivation(this);

            if (!activationConfirmed)
            {
                return;
            }
        }

        ResolveAttack(targetUnit);
    }

    private void ResolveAttack(UnitToken targetUnit)
    {
        int accuracyRoll = Random.Range(1,7);

        Debug.Log(unitData.unitName + " attacks " + targetUnit.unitData.unitName + " with a roll of " + accuracyRoll);
        
        if (accuracyRoll <= targetUnit.unitData.armorValue)
        {
            Debug.Log(unitData.unitName + " misses " + targetUnit.unitData.unitName);
            FinishAttack();

            return;
        }

        int damageRoll = Random.Range(1, unitData.damageDice + 1);
        int totalDamage = damageRoll + unitData.damageBonus;

        targetUnit.TakeDamage(totalDamage);
        hasAttackedThisActivation = true;
        FinishAttack();
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }

        Debug.Log(unitData.unitName + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            DefeatUnit();
        }
    }

    private void FinishAttack()
    {
        CancelUnitAttack();
        hasAttackedThisActivation = true;
        EndActivation();

        if (unitSelectionManager != null)
        {
            unitSelectionManager.currentUnitState = UnitState.UnitSelected;
            unitSelectionManager.RefreshUnitUI();
        }
    }

    private void DefeatUnit()
    {
        if (currentCell != null)
        {
            currentCell.occupyingUnit = null;
        }

        Debug.Log(unitData.unitName + " was defeated.");

        gameObject.SetActive(false);
    }
}
