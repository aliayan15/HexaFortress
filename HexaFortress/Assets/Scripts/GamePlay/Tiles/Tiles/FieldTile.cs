using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class FieldTile : TileBase
    {
        public int ProdusedGoldAmount { get; private set; }
        [SerializeField] private SOTileGoldData data;

        public override void Init(HexGridNode myNode)
        {
            base.Init(myNode);
            ProdusedGoldAmount = data.BaseGold;
            GameModel.Instance.PlayerData.AddGoldPerDay(ProdusedGoldAmount);
            TileManager.Instance.AddEconomyTile(this);
        }

        protected override void OnEnable()
        {

        }
        protected override void OnDisable()
        {

        }
    }
}

