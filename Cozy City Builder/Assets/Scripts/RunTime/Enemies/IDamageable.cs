
using System;

public interface IDamageable
{
    public void TakeDamage(DamageData damage);

}

public struct DamageData
{
    public int Damage;
    public int ArmorDamage;
    public int CritChance;
    public int SlowChance;
    public EnemyType TargetEnemyType;
    public DamageData(int d, int aD)
    {
        Damage = d;
        ArmorDamage = aD;
        CritChance = 0;
        SlowChance = 0;
        TargetEnemyType = EnemyType.None;
    }
}

[Flags]
public enum EnemyType
{
    None = 1,
    Ground = 2,
    Air = 4
}