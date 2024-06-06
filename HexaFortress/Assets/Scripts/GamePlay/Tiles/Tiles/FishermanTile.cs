using Managers;
using System.Collections;
using Players;
using UnityEngine;


public class FishermanTile : TileBase
{
    public int ProdusedGoldAmount { get; private set; }
    [SerializeField] private SOTileGoldData data;

    public override bool CanBuildHere(HexGridNode grid)
    {
        bool canPlace = false;
        var surroundingTiles = GridManager.Instance.GetSurroundingGrids(_myHexNode);
        foreach (var surroundingTile in surroundingTiles)
        {
            if (!surroundingTile.MyTile)
                continue;
            if (surroundingTile.MyTile.MyType == TileType.Water)
                canPlace = true;
            if (surroundingTile.MyTile.MyType == TileType.FishHouse)
                return false;
        }
        return canPlace;
    }

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        ProdusedGoldAmount = data.BaseGold;
        // if there is mill near, add extra gold
        var surroundingTiles = GridManager.Instance.GetSurroundingGrids(myNode);
        foreach (var surroundingTile in surroundingTiles)
        {
            if (!surroundingTile.MyTile)
                continue;
            if (surroundingTile.MyTile.MyType == TileType.FishTile)
                DoBonusEffect();
        }
    }

    public void DoBonusEffect()
    {
        ProdusedGoldAmount += data.BonusGold;
    }

    protected override void OnTurnStateChange(TurnStates state)
    {
        if (state != TurnStates.TurnBegin) return;
        Player.Instance.AddGold(ProdusedGoldAmount);
    }
}
