
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
}