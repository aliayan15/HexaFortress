
public interface IDamageable
{
    public void TakeDamage(DamageData damage);

}

public struct DamageData
{
    public int Damage;
    public int CritChance;
    public int SlowChance;
    public bool HaveFlyingUnitBonus;
    public DamageData(int d)
    {
        Damage = d;
        CritChance = 0;
        SlowChance = 0;
        HaveFlyingUnitBonus = false;
    }
}