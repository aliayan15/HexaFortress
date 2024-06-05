using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldTile : TileBase
{
    public int ProdusedGoldAmount { get; private set; }
    [SerializeField] private SOTileGoldData data;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        ProdusedGoldAmount = data.BaseGold;
        GameManager.Instance.player.AddGoldPerDay(ProdusedGoldAmount);
        TileManager.Instance.AddEconomyTile(this);
    }

    

    protected override void OnEnable()
    {

    }
    protected override void OnDisable()
    {

    }
}

