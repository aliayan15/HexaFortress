using UnityEngine;
using UnityEngine.Serialization;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(menuName = "ScriptableObject/GetBonus Strategy")]
    public class GetBonusStrategy : TileStrategy
    {
        [SerializeField] private TileType bonusTileType;
        [SerializeField] private TileGoldData data;
        private Transform _transform;
        private int _producedGoldAmount;
        
        public override void Init(HexGridNode myNode,TileBase tile)
        {
            _transform = tile.transform;
            _producedGoldAmount = data.BaseGold;
            GameModel.Instance.PlayerData.AddGoldPerDay(_producedGoldAmount);
            var surroundingTiles = GridManager.Instance.GetSurroundingGrids(myNode);
            foreach (var surroundingTile in surroundingTiles)
            {
                if (!surroundingTile.MyTile)
                    continue;
                if (surroundingTile.MyTile.MyType == bonusTileType)
                    DoBonusEffect();
            }
            TileManager.Instance.AddEconomyTile(tile);
        }

        public override void DoBonusEffect()
        {
            GameModel.Instance.PlayerData.AddGoldPerDay(-_producedGoldAmount);
            _producedGoldAmount += data.BonusGold;
            GameModel.Instance.PlayerData.AddGoldPerDay(_producedGoldAmount);
            Player.Instance.PlayPartical(_transform.position);
        }
    }
}