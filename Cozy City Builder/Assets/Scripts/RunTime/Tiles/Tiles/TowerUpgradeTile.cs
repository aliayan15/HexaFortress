using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerUpgradeTile : TileBase
{
    [SerializeField] private int goldExpense;
    [SerializeField] ParticleSystem smokePar;
    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
        foreach (var surroundingTile in neighbourGrids)
        {
            if (!surroundingTile.MyTile)
                continue;
            if (surroundingTile.MyTile.MyType == TileType.Tower || surroundingTile.MyTile.MyType == TileType.Cannon
                || surroundingTile.MyTile.MyType == TileType.Mortar)
            {
                TowerTileBase tower = surroundingTile.MyTile as TowerTileBase;
                tower.UpgradeDamageTower();
            }
        }
        smokePar.Play();
        GameManager.Instance.player.AddExpensesPerDay(goldExpense);
    }
}

