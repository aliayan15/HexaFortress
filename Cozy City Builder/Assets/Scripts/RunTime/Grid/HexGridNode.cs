using System;
using UnityEngine;

/// <summary>
/// Grid item data.
/// </summary>
[System.Serializable]
public class HexGridNode
{
    public Vector3 Position { get; private set; }
    public TileBase MyTile { get; private set; }
    public int x { get; private set; } = 0;
    public int y { get; private set; } = 0;
    public bool CanBuildHere { get; private set; } = false;

    public int gCost;
    public int hCost;
    public int fCost;
    public HexGridNode cameFromNode;

    public bool IsWalkable { get; private set; } = false;

    private TilePlaceHolder tilePlaceHolder = null;

    public HexGridNode(int x, int y, Vector3 position, TilePlaceHolder tilePlaceHolder)
    {
        this.x = x;
        this.y = y;
        Position = position;
        this.tilePlaceHolder = tilePlaceHolder;
    }

    public void TurnChange(bool active)
    {
        ActivetePlaceHolder(active);
    }

    public void PlaceTile(TileBase tile)
    {
        MyTile = tile;
        ActivetePlaceHolder(false);
        CanBuildHere = false;
        GridManager.Instance.OnTurnChange -= TurnChange;
    }

    public void ActivetePlaceHolder(bool active)
    {
        if (MyTile != null && active) return;
        tilePlaceHolder.gameObject.SetActive(active);
        CanBuildHere = active;
    }

    public void SetIsWalkable(bool isWalkable) => IsWalkable = isWalkable;

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return "Coor. X:" + x + ", Y:" + y;
    }
}

