using UnityEngine;

namespace HexaFortress.GamePlay
{
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
            Player.Instance.AddExpensesPerDay(goldExpense);
        }
    }
}

