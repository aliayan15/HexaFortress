using HexaFortress.Game;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(menuName = "ScriptableObject/CastleRepair Strategy")]
    public class CastleRepairStrategy : TileStrategy
    {
        [SerializeField] private UIEvents events;
        [Header("Stats")]
        [SerializeField] private short repairAmount = 1;
        [SerializeField] private int goldExpense;
        public override void Init(HexGridNode myNode, TileBase tile)
        {
            events.OnCastleToolTipChage.Invoke();
            GameModel.Instance.PlayerData.AddExpensesPerDay(goldExpense);
        }

        public override void DoBonusEffect()
        {
            
        }

        public override void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.TurnBegin)
                GameModel.Instance.CastleTile.RepairHealth(repairAmount);
        }
    }
}