using UnityEngine;

public enum UnitFaction
{
    FungalCrusaders,
    DemonicHorde,
    CarrionCrows
}

public enum UnitTier
{
    Tier1 = 1,
    Tier2 = 2,
    Tier3 = 3
}

public enum FactionAbility
{
    Regeneration,
    InvigoratingSpores,
    Swarm
}

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Identity")]
    public string unitName;
    public UnitFaction faction;
    public UnitTier tier;

    [Header("Visuals")]
    public Sprite unitPortrait;

    [Header("Stats")]
    public int currentHealth;
    public int maxHealth;
    public int movementSpeed;
    public int armorValue;
    public int attackRange;

    [Header("Damage")]
    public string damageVisual;
    public int damageDice;
    public int damageBonus;

    [Header("AP Cost")]
    public int actionPointCost = 1;

    [Header("Abilities")]
    public string passiveName;
    
    [TextArea]
    public string passiveDescription;
}
