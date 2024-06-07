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
            Player.Instance.AddGoldPerDay(ProdusedGoldAmount);
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
            Player.Instance.AddGoldPerDay(-ProdusedGoldAmount);
            ProdusedGoldAmount += data.BonusGold;
            Player.Instance.AddGoldPerDay(ProdusedGoldAmount);
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

