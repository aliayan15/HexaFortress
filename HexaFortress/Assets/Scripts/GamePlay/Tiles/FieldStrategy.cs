using UnityEngine;

namespace HexaFortress.GamePlay
{
    [CreateAssetMenu(menuName = "ScriptableObject/Field Strategy")]
    public class FieldStrategy : TileStrategy
    {
        [SerializeField] private TileType bonusTileType;
        [SerializeField] private TileGoldData data;
        public override void Init(HexGridNode myNode, TileBase tile)
        {
            GameModel.Instance.PlayerData.AddGoldPerDay(data.BaseGold);
            TileManager.Instance.AddEconomyTile(tile);
            var surroundingTiles = GridManager.Instance.GetSurroundingGrids(myNode);
            foreach (var surroundingTile in surroundingTiles)
            {
                if (!surroundingTile.MyTile)
                    continue;
                if (surroundingTile.MyTile.MyType == bonusTileType)
                    DoBonusEffect();
            }
        }

        public override void DoBonusEffect()
        {
            
        }
    }
}