
public class StatsPlayer : Stats
{
    private Inventory _inventory;

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
    }

    public override float GetStatValue(StatName stat)
    {
        return baseValues[stat] + _inventory.GetEquipmentStatBonus(stat);
    }
}