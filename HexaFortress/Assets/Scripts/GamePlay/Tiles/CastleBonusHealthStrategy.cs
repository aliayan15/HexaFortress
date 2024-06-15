using UnityEngine;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(menuName = "ScriptableObject/CastleBonusHealth Strategy")]
    public class CastleBonusHealthStrategy : TileStrategy
    {
        [Header("Stats")]
        [SerializeField] private int bonusHealth = 1;
        [SerializeField] private int goldExpense;
        public override void Init(HexGridNode myNode, TileBase tile)
        {
            GameModel.Instance.CastleTile.UpgradeHealth(bonusHealth);
            GameModel.Instance.PlayerData.AddExpensesPerDay(goldExpense);
        }

        public override void DoBonusEffect()
        {
            
        }
    }
}