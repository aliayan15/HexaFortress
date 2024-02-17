using KBCore.Refs;
using Managers;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.Grid))]
public class GridManager : MonoBehaviour
{
    public Vector2 GridSize => gridSize;

    [SerializeField, Self] private UnityEngine.Grid grid;
    [SerializeField] private SOGameProperties gameData;
    [Header("Settings")]
    [SerializeField] Vector2Int gridSize;
    [HorizontalLine]
    [Header("test")]
    [SerializeField] private TilePlaceHolder placeHolder;

    public GridData[,] _hexGridData;

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        GameManager.Instance.gridManager = this;
    }

    private void Start()
    {
        CreateHexGrid();
        CreatePlayerCastle();
    }

    private void CreatePlayerCastle()
    {
        int coordX = Mathf.FloorToInt(gridSize.x / 2);
        int coordY = Mathf.FloorToInt(gridSize.y / 2);
        var castleGrid = _hexGridData[coordX, coordY];
        CastleTile castle = Instantiate(gameData.Castle, castleGrid.Position, Quaternion.identity);
        castle.gameObject.name = "Player Castle";
        castleGrid.PlaceTile(castle);
        castle.Init(GetSurroundingGrids(castleGrid));
        CameraManager.Instance.TeleportPosition(castleGrid.Position);
    }

    private void CreateHexGrid()
    {
        // destroy childs 
        foreach (Transform item in transform)
        {
            DestroyImmediate(item.gameObject);
        }

        _hexGridData = new GridData[gridSize.x, gridSize.y];
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 worldPos = grid.GetCellCenterWorld(new Vector3Int(x, y));
                TilePlaceHolder tilePlaceHolder = Instantiate(placeHolder, worldPos, Quaternion.identity, transform);
                _hexGridData[x, y] = new GridData(x, y, worldPos, tilePlaceHolder);
                tilePlaceHolder.gameObject.SetActive(false);
            }
        }
    }

    public List<GridData> GetSurroundingGrids(GridData grid)
    {
        List<GridData> result = new List<GridData>();
        GridData[] grids = new GridData[6];
        int x = grid.x, y = grid.y;
        grids[0] = GetGridDataByIndex(x - 1, y - 1);
        grids[1] = GetGridDataByIndex(x, y - 1);
        grids[2] = GetGridDataByIndex(x + 1, y);
        grids[3] = GetGridDataByIndex(x, y + 1);
        grids[4] = GetGridDataByIndex(x - 1, y + 1);
        grids[5] = GetGridDataByIndex(x - 1, y);
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
    public GridData GetGridItem(Vector3 worldPos)
    {
        GetGridCoordinate(worldPos, out int x, out int y);
        return GetGridDataByIndex(x, y);
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
    private GridData GetGridDataByIndex(int x, int y)
    {
        bool isValidIndex = x >= 0 && y >= 0 && x < _hexGridData.GetLength(0) && y < _hexGridData.GetLength(1);
        if (!isValidIndex) return null;
        return _hexGridData[x, y];
    }
}

/// <summary>
/// Grid item data.
/// </summary>
[System.Serializable]
public class GridData
{
    public Vector3 Position { get; private set; }
    public TileBase MyTile { get; private set; }
    public int x { get; private set; } = 0;
    public int y { get; private set; } = 0;

    private TilePlaceHolder tilePlaceHolder = null;

    public GridData(int x, int y, Vector3 position, TilePlaceHolder tilePlaceHolder)
    {
        this.x = x;
        this.y = y;
        Position = position;
        this.tilePlaceHolder = tilePlaceHolder;
    }
    public void PlaceTile(TileBase tile)
    {
        MyTile = tile;
        ActivetePlaceHolder(false);
    }

    public void ActivetePlaceHolder(bool active)
    {
        if (MyTile != null && active) return;
        tilePlaceHolder.gameObject.SetActive(active);
    }

    public override string ToString()
    {
        return "Coor. X:" + x + ", Y:" + y;
    }
}

