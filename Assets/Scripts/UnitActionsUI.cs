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

    [Header("Unit Card")]
    public GameObject frontPanel;
    public GameObject backPanel;

    [Header("Unit Information")]
    public Image    unitPortraitImage;
    public TMP_Text unitNameText;
    public TMP_Text healthText;
    public TMP_Text armorText;
    public TMP_Text movementSpeedText;
    public TMP_Text damageDiceText;
    public TMP_Text attackRangeText;
    public TMP_Text apCostText;

    public TMP_Text unitFactionText;
    public TMP_Text unitTierText;
    public TMP_Text unitPassiveNameText;
    public TMP_Text unitPassiveDescriptionText;

    [Header("Buttons")]
    public GameObject attackButton;
    public GameObject moveButton;
    public GameObject abilityButton;

    private bool showingBack = false;

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

        showingBack = false;
        frontPanel.SetActive(true);
        backPanel.SetActive(false);

        unitPortraitImage.sprite = unit.unitData.unitPortrait;

        unitNameText.text = unit.unitData.unitName;

        healthText.text = "HP: " + unit.currentHealth + " / " + unit.unitData.maxHealth;

        armorText.text = "Armor: " + unit.unitData.armorValue;

        movementSpeedText.text = "Movement: " + unit.unitData.movementSpeed;

        damageDiceText.text = "Damage: " + unit.unitData.damageVisual;

        attackRangeText.text = "Range: " + unit.unitData.attackRange;

        apCostText.text = "AP Cost: " + unit.unitData.actionPointCost;

        unitFactionText.text = "-" + unit.unitData.faction + "-";

        unitTierText.text = unit.unitData.tier.ToString();

        unitPassiveNameText.text = unit.unitData.passiveName;

        unitPassiveDescriptionText.text = unit.unitData.passiveDescription;

        UpdateActionButtons(unit);
    }

    public void FlipUnitCard()
    {
        showingBack = !showingBack;
        frontPanel.SetActive(!showingBack);
        backPanel.SetActive(showingBack);
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

        if (selectionManager.currentUnitState == UnitState.UnitMoving)
        {
            unit.CancelUnitMovement();
            selectionManager.currentUnitState = UnitState.UnitSelected;

            return;
        }

        if (selectionManager.currentUnitState == UnitState.UnitAttacking)
        {
            unit.CancelUnitAttack();
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

    public void AttackSelectedUnit()
    {
        UnitToken unit = selectionManager.selectedUnit;

        if (unit == null)
        {
            return;
        }

        if (selectionManager.currentUnitState == UnitState.UnitAttacking)
        {
            unit.CancelUnitAttack();
            selectionManager.currentUnitState = UnitState.UnitSelected;

            return;
        }

        if (selectionManager.currentUnitState == UnitState.UnitMoving)
        {
            unit.CancelUnitMovement();
        }

        if (unit.hasAttackedThisActivation)
        {
            Debug.Log("This unit has already attacked this activation.");
            return;
        }

        if (!selectionManager.CanStartActivation(unit))
        {
            Debug.Log("Not enough action points for " + unit.unitData.unitName);
            return;
        }

        selectionManager.currentUnitState = UnitState.UnitAttacking;
        unit.BeginUnitAttack();

    }
}