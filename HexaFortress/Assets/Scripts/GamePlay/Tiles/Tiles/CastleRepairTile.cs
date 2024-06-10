using HexaFortress.Game;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    /// <summary>
    /// Repair castle health every day.
    /// </summary>
    public class CastleRepairTile : TileBase
    {
        [SerializeField] private UIEvents events;
        [Header("Stats")]
        [SerializeField] private short repairAmount = 1;
        [SerializeField] private int goldExpense;

        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            events.OnCastleToolTipChage.Invoke();
            GameModel.Instance.PlayerData.AddExpensesPerDay(goldExpense);
        }

        protected override void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.TurnBegin)
                GameModel.Instance.CastleTile.RepairHealth(repairAmount);
        }
    }
}

