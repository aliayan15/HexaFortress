using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTileBase : TileBase, ITowerUpgradeable
{
    [Header("Settings")]
    [SerializeField] protected float fireTime;
    [SerializeField] protected int baseDamage;
    [SerializeField] protected int damageUpgrade;
    [SerializeField] protected LayerMask enemyLayer;

    private WaitForSeconds _fireTimer;
    private bool _canFire = false;
    protected DamageData _damageData;


    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        _fireTimer = new WaitForSeconds(fireTime);
        _damageData = new DamageData(baseDamage);
        StartCoroutine(FireTimer());
        CheckUpgradeTile();
    }

    private void CheckUpgradeTile()
    {
        var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
        foreach (var surroundingTile in neighbourGrids)
        {
            if (!surroundingTile.MyTile)
                continue;
            if (surroundingTile.MyTile.MyType == TileType.TowerUpgrade)
                UpgradeTower();
        }
    }

    #region Upgrade
    public void SetFlyingUnitBonus(bool haveBonus)
    {
        _damageData.HaveFlyingUnitBonus = haveBonus;
    }

    public void UpgradeCritEffect()
    {
        _damageData.CritChance += 10;
    }

    public void UpgradeSlowEffect()
    {
        _damageData.SlowChance += 10;
    }

    public void UpgradeTower()
    {
        _damageData.Damage += damageUpgrade;
    }
    #endregion

    #region Fire
    protected IEnumerator FireTimer()
    {
        while (true)
        {
            yield return _fireTimer;
            if (_canFire)
            {
                OnFire();
            }
        }
    }

    protected virtual void OnFire()
    {

    }
    #endregion

    protected override void OnTurnStateChange(TurnStates state)
    {
        _canFire = state == TurnStates.EnemySpawnStart;
    }
}
