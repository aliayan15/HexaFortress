using HexaFortress.Game;
using UnityEngine;

namespace HexaFortress.GamePlay
{
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
            // TODO UIManager.Instance.gameCanvasManager.UpdateCastleToolTip();
            Player.Instance.AddExpensesPerDay(goldExpense);
        }

        protected override void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.TurnBegin)
                GridManager.Instance.PlayerCastle.RepairHealth(repairAmount);
        }
    }
}

