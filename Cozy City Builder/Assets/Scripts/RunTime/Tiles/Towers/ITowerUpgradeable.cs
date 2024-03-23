using System.Collections;
using UnityEngine;


public interface ITowerUpgradeable
{
    public void UpgradeSlowEffect();
    public void UpgradeCritEffect();
    public void DamageUpgradeTower();
    public void UpgradeArmorDamage();
    public void SetEnemyTypeBonus(EnemyType type);

}
