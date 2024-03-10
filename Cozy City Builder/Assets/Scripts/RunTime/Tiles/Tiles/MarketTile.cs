using Managers;
using UnityEngine;


public class MarketTile : TileBase,ITileBonusEffect
{
    public int ProdusedGoldAmount { get; private set; }
    [SerializeField] private SOTileGoldData data;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
        ProdusedGoldAmount = data.BaseGold;
        GameManager.Instance.player.AddGoldPerDay(ProdusedGoldAmount);
        foreach (var surroundingTile in neighbourGrids)
        {
            if (!surroundingTile.MyTile)
                continue;
            if (surroundingTile.MyTile.MyType == TileType.House)
                DoBonusEffect();
        }
    }

    public void DoBonusEffect()
    {
        GameManager.Instance.player.AddGoldPerDay(-ProdusedGoldAmount);
        ProdusedGoldAmount += data.BonusGold;
        GameManager.Instance.player.AddGoldPerDay(ProdusedGoldAmount);
        GameManager.Instance.player.PlayPartical(transform.position);
    }
    protected override void OnEnable()
    {
        
    }
    protected override void OnDisable()
    {
        
    }
}

