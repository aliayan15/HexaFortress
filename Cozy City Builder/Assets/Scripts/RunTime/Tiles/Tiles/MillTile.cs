using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MillTile : TileBase
{
    public int ProdusedGoldAmount { get; private set; }
    [SerializeField] private SOTileGoldData data;
    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        ProdusedGoldAmount = data.BaseGold;
        GameManager.Instance.player.AddGoldPerDay(ProdusedGoldAmount);
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
        GameManager.Instance.player.AddGoldPerDay(-ProdusedGoldAmount);
        ProdusedGoldAmount += data.BonusGold;
        GameManager.Instance.player.AddGoldPerDay(ProdusedGoldAmount);
    }

    protected override void OnDisable()
    {

    }
    protected override void OnEnable()
    {

    }
}

