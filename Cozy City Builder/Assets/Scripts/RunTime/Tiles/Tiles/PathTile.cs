using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class PathTile : TileBase
{
    public bool IsSpawnPoint { get; private set; } = false;

    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        myNode.SetIsWalkable(true);
        CheckSpawnPoints();
    }

    private void CheckSpawnPoints()
    {
        var neighbourGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
        foreach (var grid in neighbourGrids)
        {
            if (!grid.MyTile) continue;
            if (grid.MyTile.MyType != TileType.Path) continue;
            PathTile path = grid.MyTile as PathTile;
            if (!path.IsSpawnPoint) continue;
            var pathToCastle = GridManager.Instance.PathFinding.FindPath(transform.position,
                GridManager.Instance.PlayerCastle.MyHexNode.Position);
            int myPathCount = pathToCastle.Count;
            pathToCastle = GridManager.Instance.PathFinding.FindPath(grid.Position,
                GridManager.Instance.PlayerCastle.MyHexNode.Position);
            if (myPathCount > pathToCastle.Count)
            {
                path.SetSpawnPoint(false);
                SetSpawnPoint(true);
            }
            if (myPathCount == pathToCastle.Count)
            {
                SetSpawnPoint(true);
            }
        }
    }

    public void SetSpawnPoint(bool isSpawnPoint)
    {
        IsSpawnPoint = isSpawnPoint;
        if (IsSpawnPoint)
            TileManager.Instance.AddEnemySpawnPoint(transform.position);
        else
            TileManager.Instance.RemoveEnemySpawnPoint(transform.position);

    }

    public override bool CanBuildHere(HexGridNode grid)
    {
        var surroundingGrids = GridManager.Instance.GetSurroundingGrids(grid);
        bool build = false;
        foreach (var item in surroundingGrids)
        {
            if (!item.MyTile) continue;
            if (item.MyTile.MyType == TileType.Castle)
                return false;
            if (item.MyTile.MyType == TileType.Path)
                build = true;
        }
        return build;
    }

    protected override void OnDisable()
    {

    }
    protected override void OnEnable()
    {

    }
}

