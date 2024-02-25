using KBCore.Refs;
using Managers;
using MyUtilities;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.Grid))]
public class GridManager : SingletonMono<GridManager>
{
    public Vector2 GridSize => gridSize;
    public HexPathFinding PathFinding { get; private set; }
    public CastleTile PlayerCastle { get; private set; }

    [SerializeField, Self] private UnityEngine.Grid grid;
    [SerializeField] private SOGameProperties gameData;
    [Header("Settings")]
    [SerializeField] Vector2Int gridSize;
    [HorizontalLine]
    [Header("Refs")]
    [SerializeField] private TilePlaceHolder placeHolder;
    [SerializeField] private Transform floor;

    public HexGridNode[,] _hexGrid;

    private void OnValidate()
    {
        this.ValidateRefs();
    }


    private IEnumerator Start()
    {
        CreateHexGrid();
        CreatePlayerCastle();
        PathFinding = new HexPathFinding(this, gridSize);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.SetTurnState(TurnStates.TurnBegin);
    }

    private void CreatePlayerCastle()
    {
        int coordX = Mathf.FloorToInt(gridSize.x / 2);
        int coordY = Mathf.FloorToInt(gridSize.y / 2);
        var castleGrid = _hexGrid[coordX, coordY];
        CastleTile castle = Instantiate(gameData.Castle, castleGrid.Position, Quaternion.identity);
        castle.gameObject.name = "Player Castle";
        castle.Init(castleGrid);
        CameraManager.Instance.TeleportPosition(castleGrid.Position);
        PlayerCastle = castle;
        // create one path
        var pathNode = GetGridNode(castle.PathPoint.position);
        PathTile path = Instantiate(gameData.PathTile, pathNode.Position, Quaternion.identity);
        path.Init(pathNode);
        floor.transform.position = castleGrid.Position;
    }

    private void CreateHexGrid()
    {
        // destroy childs 
        foreach (Transform item in transform)
        {
            DestroyImmediate(item.gameObject);
        }

        _hexGrid = new HexGridNode[gridSize.x, gridSize.y];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 worldPos = grid.GetCellCenterWorld(new Vector3Int(x, y));
                TilePlaceHolder tilePlaceHolder = Instantiate(placeHolder, worldPos, Quaternion.identity, transform);
                _hexGrid[x, y] = new HexGridNode(x, y, worldPos, tilePlaceHolder);
                tilePlaceHolder.gameObject.SetActive(false);
            }
        }
    }

    public List<HexGridNode> GetSurroundingGrids(HexGridNode grid)
    {
        List<HexGridNode> result = new List<HexGridNode>();
        HexGridNode[] grids = new HexGridNode[6];
        int x = grid.x, y = grid.y;
        grids[2] = GetGridNode(x + 1, y);
        grids[5] = GetGridNode(x - 1, y);
        grids[3] = GetGridNode(x, y + 1);
        grids[1] = GetGridNode(x, y - 1);

        bool oddRow = grid.y % 2 == 1;
        if (oddRow)
        {
            grids[0] = GetGridNode(x + 1, y + 1);
            grids[4] = GetGridNode(x + 1, y - 1);
        }
        else
        {
            grids[0] = GetGridNode(x - 1, y + 1);
            grids[4] = GetGridNode(x - 1, y - 1);
        }


        for (int i = 0; i < grids.Length; i++)
        {
            if (grids[i] != null)
                result.Add(grids[i]);
        }

        return result;
    }

    /// <summary>
    /// Return grid data in given position or null.
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public HexGridNode GetGridNode(Vector3 worldPos)
    {
        GetGridCoordinate(worldPos, out int x, out int y);
        return GetGridNode(x, y);
    }

    public void GetGridCoordinate(Vector3 worldPos, out int x, out int y)
    {
        Vector3Int coord = grid.WorldToCell(worldPos);
        x = coord.x;
        y = coord.y;
    }

    /// <summary>
    /// Return grid data in given index or null.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public HexGridNode GetGridNode(int x, int y)
    {
        bool isValidIndex = x >= 0 && y >= 0 && x < _hexGrid.GetLength(0) && y < _hexGrid.GetLength(1);
        if (!isValidIndex) return null;
        return _hexGrid[x, y];
    }

}

