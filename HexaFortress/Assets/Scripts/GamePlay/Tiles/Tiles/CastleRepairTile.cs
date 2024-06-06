using Managers;
using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;

/// <summary>
/// Repair castle health every day.
/// </summary>
public class CastleRepairTile : TileBase
{
    [Header("Stats")]
    [SerializeField] private short repairAmount = 1;
    [SerializeField] private int goldExpense;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        UIManager.Instance.gameCanvasManager.UpdateCastleToolTip();
        Player.Instance.AddExpensesPerDay(goldExpense);
    }

    protected override void OnTurnStateChange(TurnStates state)
    {
        if (state == TurnStates.TurnBegin)
            GridManager.Instance.PlayerCastle.RepairHealth(repairAmount);
    }
}

