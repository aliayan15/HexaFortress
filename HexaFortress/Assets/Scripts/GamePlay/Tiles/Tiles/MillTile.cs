using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class MillTile : TileBase
    {
        public int ProdusedGoldAmount { get; private set; }
        [SerializeField] private SOTileGoldData data;
        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            ProdusedGoldAmount = data.BaseGold;
            Player.Instance.AddGoldPerDay(ProdusedGoldAmount);
            // if there is fielt near, add extra gold
            var surroundingTiles = GridManager.Instance.GetSurroundingGrids(myNode);
            foreach (var surroundingTile in surroundingTiles)
            {
                if (!surroundingTile.MyTile)
                    continue;
                if (surroundingTile.MyTile.MyType == TileType.Fielts)
                    DoBonusEffect();

            }
            TileManager.Instance.AddEconomyTile(this);
        }

        private void DoBonusEffect()
        {
            Player.Instance.AddGoldPerDay(-ProdusedGoldAmount);
            ProdusedGoldAmount += data.BonusGold;
            Player.Instance.AddGoldPerDay(ProdusedGoldAmount);
            Player.Instance.PlayPartical(transform.position);
        }

        protected override void OnDisable()
        {

        }
        protected override void OnEnable()
        {

        }
    }
}

