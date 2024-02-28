using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    None,
    Path,
    House,
    Forest,
    Water,
    Tower,
    Castle,
    Windmill,
    Mountain,
    FishHouse,
    Fielts,
    Grass,
    FishTile
}


public abstract class TileBase : MonoBehaviour
{
    [SerializeField] protected TileType tileType;

    public TileType MyType => tileType;
    public int CurrentRos { get; set; } = 0;
    public HexGridNode MyHexNode => _myHexNode;

    protected HexGridNode _myHexNode;


    public void Rotate(bool left = false)
    {
        float rosY = 0;
        if (left)
        {
            rosY = SOGameProperties.GetPreviousRotation(CurrentRos, out int index);
            CurrentRos = index;
        }
        else
        {
            rosY = SOGameProperties.GetNextRotation(CurrentRos, out int index);
            CurrentRos = index;
        }

        transform.rotation = Quaternion.Euler(0, rosY, 0);
    }

    /// <summary>
    /// Invoking after placing the tile.
    /// </summary>
    /// <param name="surroundingGrids"></param>
    public virtual void Init(HexGridNode myNode)
    {
        myNode.PlaceTile(this);
        _myHexNode = myNode;
        CheckSurroundingGrids();
    }

    protected virtual void CheckSurroundingGrids()
    {
        var surroundingGrids = GridManager.Instance.GetSurroundingGrids(_myHexNode);
        foreach (var grid in surroundingGrids)
        {
            grid.ActivetePlaceHolder(true);
        }
    }

    public virtual bool CanBuildHere(HexGridNode grid)
    {
        return true;
    }

    protected virtual void OnTurnStateChange(TurnStates state)
    {

    }

    protected virtual void OnEnable()
    {
        GameManager.OnTurnStateChange += OnTurnStateChange;
    }
    protected virtual void OnDisable()
    {
        GameManager.OnTurnStateChange -= OnTurnStateChange;
    }
}

