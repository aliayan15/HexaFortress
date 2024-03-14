using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HouseTile : TileBase
{
    [SerializeField] ParticleSystem smokePar;
    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
        foreach (var surroundingTile in neighbourGrids)
        {
            if (!surroundingTile.MyTile)
                continue;
            if (surroundingTile.MyTile.MyType == TileType.Market)
            {
                MarketTile market = surroundingTile.MyTile as MarketTile;
                market.DoBonusEffect();
            }
        }
        smokePar.Play();
    }
}

