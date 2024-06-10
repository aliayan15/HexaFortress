using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class MarketTile : TileBase,ITileBonusEffect
    {
        public int ProdusedGoldAmount { get; private set; }
        [SerializeField] private SOTileGoldData data;

        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
            ProdusedGoldAmount = data.BaseGold;
            GameModel.Instance.PlayerData.AddGoldPerDay(ProdusedGoldAmount);
            foreach (var surroundingTile in neighbourGrids)
            {
                if (!surroundingTile.MyTile)
                    continue;
                if (surroundingTile.MyTile.MyType == TileType.House)
                    DoBonusEffect();
            }
            TileManager.Instance.AddEconomyTile(this);
        }

        public void DoBonusEffect()
        {
            GameModel.Instance.PlayerData.AddGoldPerDay(-ProdusedGoldAmount);
            ProdusedGoldAmount += data.BonusGold;
            GameModel.Instance.PlayerData.AddGoldPerDay(ProdusedGoldAmount);
            Player.Instance.PlayPartical(transform.position);
        }
        protected override void OnEnable()
        {
        
        }
        protected override void OnDisable()
        {
        
        }
    }
}

