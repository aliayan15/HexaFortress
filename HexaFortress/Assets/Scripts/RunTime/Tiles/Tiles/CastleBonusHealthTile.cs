using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Increase castle max health.
/// </summary>
public class CastleBonusHealthTile : TileBase
{
    [Header("Stats")]
    [SerializeField] private int bonusHealth = 1;
    [SerializeField] private int goldExpense;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        GridManager.Instance.PlayerCastle.UpgradeHealth(bonusHealth);
        GameManager.Instance.player.AddExpensesPerDay(goldExpense);
    }
}

