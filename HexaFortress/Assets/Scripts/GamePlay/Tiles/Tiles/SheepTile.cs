using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class SheepTile : TileBase, ITileBonusEffect
    {
        public int ProdusedGoldAmount { get; private set; }
        [SerializeField] private SOTileGoldData data;

        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            ProdusedGoldAmount = data.BaseGold;
            Player.Instance.AddGoldPerDay(ProdusedGoldAmount);
            var surroundingTiles = GridManager.Instance.GetSurroundingGrids(myNode);
            foreach (var surroundingTile in surroundingTiles)
            {
                if (!surroundingTile.MyTile)
                    continue;
                if (surroundingTile.MyTile.MyType == TileType.Grass)
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

