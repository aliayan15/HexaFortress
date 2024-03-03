using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class PathTile : TileBase
{
    public Transform[] ConnectionPoints => connectionPoints;

    [Header("Path")]
    [SerializeField] private Transform[] connectionPoints;
    [Space(5)]
    [SerializeField] private Transform[] spawnPoints;


    public override void Init(HexGridNode myNode)
    {
        base.Init(myNode);
        myNode.SetIsWalkable(true);
        CheckSpawnPoints();
    }

    private void CheckSpawnPoints()
    {
        List<PathTile> neighbourPaths = new List<PathTile>();
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            TileManager.Instance.AddEnemySpawnPoint(spawnPoints[i].position);
            var node = GridManager.Instance.GetGridNode(connectionPoints[i].position);
            if (!node.MyTile) continue;
            if (node.MyTile.MyType != TileType.Path && node.MyTile.MyType != TileType.Castle) continue;

            // baglant� notas�ndaki spawn point ��kar
            PathTile tile = node.MyTile as PathTile;
            if (tile != null)
                tile.RemoveSpawnPoint(MyHexNode);
            TileManager.Instance.RemoveEnemySpawnPoint(spawnPoints[i].position); // connection point
        }
    }

    public void RemoveSpawnPoint(HexGridNode path)
    {
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            var node = GridManager.Instance.GetGridNode(connectionPoints[i].position);
            if (node == path)
                TileManager.Instance.RemoveEnemySpawnPoint(spawnPoints[i].position);
        }
    }

    public override bool CanBuildHere(HexGridNode grid)
    {
        var surroundingGrids = GridManager.Instance.GetSurroundingGrids(grid);
        foreach (var item in surroundingGrids)
        {
            if (!item.MyTile) continue;
            if (item.MyTile.MyType == TileType.Path)
                return true;
        }
        return false;
    }

    protected override void OnDisable()
    {

    }
    protected override void OnEnable()
    {

    }
}
