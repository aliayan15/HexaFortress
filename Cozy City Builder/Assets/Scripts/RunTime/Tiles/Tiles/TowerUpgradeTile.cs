using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class TowerUpgradeTile : TileBase
{
    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
        foreach (var surroundingTile in neighbourGrids)
        {
            if (!surroundingTile.MyTile)
                continue;
            if (surroundingTile.MyTile.MyType == TileType.Tower)
            {
                TowerTileBase market = surroundingTile.MyTile as TowerTileBase;
                market.UpgradeTower();
            }
        }
    }
}

