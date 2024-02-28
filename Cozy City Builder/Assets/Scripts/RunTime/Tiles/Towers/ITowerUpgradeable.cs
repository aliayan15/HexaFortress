using System.Collections;
using UnityEngine;


public interface ITowerUpgradeable
{
    public void UpgradeSlowEffect();
    public void UpgradeCritEffect();
    public void UpgradeTower();
    public void SetFlyingUnitBonus(bool haveBonus);

}
