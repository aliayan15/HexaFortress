using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class PathTile : TileBase
{
    public bool IsSpawnPoint { get; private set; } = false;
    private PathTile _lastNearPath;

    private bool _debug;

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
            var pathToCastle = GridManager.Instance.PathFinding.FindPath(_myHexNode.Position,
                GridManager.Instance.PlayerCastle.MyHexNode.Position);

            if (pathToCastle == null) return;
            int myPathCount = pathToCastle.Count;
            pathToCastle = GridManager.Instance.PathFinding.FindPath(grid.Position,
                GridManager.Instance.PlayerCastle.MyHexNode.Position);
            if (myPathCount > pathToCastle.Count)
            {
                path.SetSpawnPoint(false);
                SetSpawnPoint(true);
                _lastNearPath = path;
                break;
            }
            if (myPathCount == pathToCastle.Count)
            {
                SetSpawnPoint(true);
                break;
            }
        }
    }

    public void SetSpawnPoint(bool isSpawnPoint)
    {
        IsSpawnPoint = isSpawnPoint;
        if (IsSpawnPoint)
            TileManager.Instance.AddEnemySpawnPoint(_myHexNode.Position);
        else
            TileManager.Instance.RemoveEnemySpawnPoint(_myHexNode.Position);

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

    public override void OnPlayerHand(bool onHand)
    {
        PathTrace.Instance.DrawPath(onHand);
    }
    public override void OnPlayerInsert(bool onHand, HexGridNode myNode)
    {
        OnPlayerHand(false);
        if (_lastNearPath != null)
        {
            _lastNearPath.SetSpawnPoint(true);
            _lastNearPath = null;
        }

        if (onHand)
        {
            if (IsSpawnPoint) // already in spawn list
                SetSpawnPoint(false);
            if (!CanBuildHere(myNode)) // cant place there
            {
                OnPlayerHand(true);
                _debug = true;
                return;
            }

            _myHexNode = myNode;
            _myHexNode.SetIsWalkable(true);
            CheckSpawnPoints();
            _myHexNode.SetIsWalkable(false);
        }
        else
        {
            if (_myHexNode == null)
            {
                OnPlayerHand(true);
                return;
            }
            SetSpawnPoint(false);
            _myHexNode.SetIsWalkable(false);
            _myHexNode = null;
        }

        OnPlayerHand(true);
    }

    protected override void OnDisable()
    {

    }
    protected override void OnEnable()
    {

    }
}

