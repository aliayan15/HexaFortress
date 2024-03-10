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
        GameManager.Instance.player.AddGoldPerDay(ProdusedGoldAmount);
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

