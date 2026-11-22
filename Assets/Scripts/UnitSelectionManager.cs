using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    [Header("Managers")]
    public TurnManager turnManager;
    public UnitActionUI unitActionUI;

    [Header("Selection")]
    public UnitToken selectedUnit;

    [Header("Unit States")]
    public UnitState currentUnitState = UnitState.None;

    public void SelectUnit(UnitToken unit)
    {
        if (unit == null)
        {
            return;
        }

        if (!turnManager.IsPlayerTurn(unit.owner))
        {
            Debug.Log("You do not own this unit.");
            return;
        }

        if (selectedUnit == unit)
        {
            ClearSelection();
            return;
        }

        if (selectedUnit != null && selectedUnit != unit)
        {
            if (selectedUnit.activationStarted)
            {
                selectedUnit.EndActivation();
            }
            else
            {
                selectedUnit.CancelUnitMovement();
                selectedUnit.CancelUnitAttack();
            }

            selectedUnit.SetSelected(false);
        }

        selectedUnit = unit;
        selectedUnit.SetSelected(true);
        currentUnitState = UnitState.UnitSelected;

        if (unitActionUI != null)
        {
            unitActionUI.ShowUnit(selectedUnit);
        }

        Debug.Log("Selected: " + selectedUnit.unitData.unitName);
    }

    public void ClearSelection()
    {
        if (selectedUnit != null)
        {
            if (selectedUnit.activationStarted)
            {
                selectedUnit.EndActivation();
            }
            else
            {
                selectedUnit.CancelUnitMovement();
                selectedUnit.CancelUnitAttack();
            }

            selectedUnit.SetSelected(false);

        }

        if (unitActionUI != null)
        {
            unitActionUI.HideUnitUI();
        }

        selectedUnit = null;
        currentUnitState = UnitState.None;

        Debug.Log("Selection cleared.");
    }

    public bool CanStartActivation(UnitToken unit)
    {
        if (unit == null)
        {
            return false;
        }

        if (unit.activationStarted)
        {
            return true;
        }

        return turnManager.canSpendActionPoints(unit.unitData.actionPointCost);
    }

    public bool ConfirmActivation(UnitToken unit)
    {
        if (unit == null)
        {
            return false;
        }

        if (unit.activationStarted)
        {
            return true;
        }

        int cost = unit.unitData.actionPointCost;

        if (!turnManager.spendActionPoints(cost))
        {
            return false;
        }

        unit.StartActivation();
        Debug.Log(unit.unitData.unitName + " activation started for " + cost + " AP.");

        return true;
    }

    public void RefreshUnitUI()
    {
        if (selectedUnit != null && unitActionUI != null)
        {
            unitActionUI.ShowUnit(selectedUnit);
        }
    }
}
