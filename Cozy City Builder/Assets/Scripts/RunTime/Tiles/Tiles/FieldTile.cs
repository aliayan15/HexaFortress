using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldTile : TileBase,ITileBonusEffect
{
    public int ProdusedGoldAmount { get; private set; }
    [SerializeField] private SOTileGoldData data;

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
            if (surroundingTile.MyTile.MyType == TileType.Windmill)
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
        GameManager.Instance.player.AddGold(ProdusedGoldAmount);
    }
}

