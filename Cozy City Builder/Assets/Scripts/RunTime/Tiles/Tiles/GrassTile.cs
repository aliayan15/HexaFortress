using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class GrassTile : TileBase
{
    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        // if there is fielt near, add extra gold
        var surroundingTiles = GridManager.Instance.GetSurroundingGrids(myNode);
        foreach (var surroundingTile in surroundingTiles)
        {
            if (!surroundingTile.MyTile)
                continue;
            if (surroundingTile.MyTile.MyType == TileType.Sheep)
            {
                if (surroundingTile.MyTile.TryGetComponent(out ITileBonusEffect tileBonusEffect))
                    tileBonusEffect.DoBonusEffect();
            }

        }
    }
    protected override void OnDisable()
    {

    }
    protected override void OnEnable()
    {

    }
}

