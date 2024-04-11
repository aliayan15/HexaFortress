using System.Collections;
using UnityEngine;


public interface ITowerUpgradeable
{
    public void UpgradeSlowEffect();
    public void UpgradeCritEffect();
    public void UpgradeDamageTower();
    public void UpgradeArmorDamage();
    public void UpgradeHealthDamage();
    public void SetEnemyTypeBonus(EnemyType type);

}
