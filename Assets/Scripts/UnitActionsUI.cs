using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitActionUI : MonoBehaviour
{
    [Header("Managers")]
    public UnitSelectionManager selectionManager;
    public TurnManager turnManager;

    [Header("Panels")]
    public GameObject unitInfoPanel;
    public GameObject unitActionPanel;

    [Header("Unit Information")]
    public TMP_Text unitNameText;
    public TMP_Text healthText;
    public TMP_Text armorText;
    public TMP_Text movementSpeedText;
    public TMP_Text damageDiceText;
    public TMP_Text attackRangeText;
    public TMP_Text apCostText;

    [Header("Buttons")]
    public GameObject attackButton;
    public GameObject moveButton;
    public GameObject abilityButton;

    private void Start()
    {
        HideUnitUI();
    }

    public void ShowUnit(UnitToken unit)
    {
        if (unit == null)
        {
            HideUnitUI();
            return;
        }

        unitInfoPanel.SetActive(true);
        unitActionPanel.SetActive(true);

        unitNameText.text = unit.unitData.unitName;

        healthText.text = "HP: " + unit.currentHealth + " / " + unit.unitData.maxHealth;

        armorText.text = "Armor: " + unit.unitData.armorValue;

        movementSpeedText.text = "Movement: " + unit.unitData.movementSpeed;

        damageDiceText.text = "Damage: " + unit.unitData.damageVisual;

        attackRangeText.text = "Range: " + unit.unitData.attackRange;

        apCostText.text = "AP Cost: " + unit.unitData.actionPointCost;

        UpdateActionButtons(unit);
    }

    public void HideUnitUI()
    {
        unitInfoPanel.SetActive(false);
        unitActionPanel.SetActive(false);
    }

    public void HidePanel()
    {
        unitActionPanel.SetActive(false);
    }

    public void MoveSelectedUnit()
    {
        UnitToken unit = selectionManager.selectedUnit;

        if (unit == null)
        {
            return;
        }

        if (unit.hasMovedThisActivation)
        {
            Debug.Log(unit.unitData.unitName + " has already moved this activation.");
            return;
        }

        if (!selectionManager.CanStartActivation(unit))
        {
            Debug.Log("Not enough action points for " + unit.unitData.unitName);

            return;
        }

        selectionManager.currentUnitState = UnitState.UnitMoving;
        unit.BeginUnitMovement();

    }

    public void UpdateActionButtons(UnitToken unit)
    {
        if (unit == null)
        {
            return;
        }

        moveButton.SetActive(!unit.hasMovedThisActivation);
        attackButton.SetActive(!unit.hasAttackedThisActivation);

    }

}