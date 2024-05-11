using Managers;
using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSelector : SingletonMono<TileSelector>
{
    [SerializeField] private List<SOTileData> tiles;
    [Tooltip("1: Free, 0: Price")]
    [SerializeField] private List<SOTileData> pathTiles;
    [SerializeField] private UnlockTile[] tilesToUnlock;
    [Space(10)]
    [SerializeField] private SOTileData[] startTiles;
    [SerializeField] private List<SOTileData> towers;

    public List<SOTileData> Towers => towers;

    private List<SOTileData> _openTilesPricelist = new List<SOTileData>();
    private List<SOTileData> _openPathTilesList = new List<SOTileData>();

    private void Start()
    {
        ResetList();
    }

    private void ResetList()
    {
        _openTilesPricelist = tiles.ToList();
        ConstractPathList();
    }

    private void ConstractPathList()
    {
        _openPathTilesList.Clear();
        _openPathTilesList = pathTiles.ToList();
        _openPathTilesList.Shuffle();
    }

    public SOTileData GetTowerTile()
    {
        return towers[Random.Range(0, towers.Count)];
    }

    public SOTileData GetTileWithPrice()
    {
        int index = Random.Range(0, _openTilesPricelist.Count);
        var tile = _openTilesPricelist[index];
        _openTilesPricelist.RemoveAt(index);
        if (_openTilesPricelist.Count == 0)
        {
            _openTilesPricelist = tiles.ToList();
            _openTilesPricelist.Shuffle();
        }

        return tile;
    }

    public SOTileData GetPathTile()
    {
        int index = Random.Range(0, _openPathTilesList.Count);
        var tile = _openPathTilesList[index];
        _openPathTilesList.RemoveAt(index);
        if (_openPathTilesList.Count == 3)
            ConstractPathList();

        return tile;
    }

    public SOTileData[] GetStartTiles()
    {
        var list = new SOTileData[4];
        list[3] = startTiles[0];
        var startList = startTiles.ToList();
        startList.Shuffle();
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, startList.Count);
            list[i] = startList[index];
            startList.RemoveAt(index);
        }
        return list;
    }

    #region State Change
    private void OnTurnStateChange(TurnStates state)
    {
        if (state == TurnStates.TurnBegin)
        {
            foreach (var tile in tilesToUnlock)
            {
                if (GameManager.Instance.DayCount == tile.UnlockDay)
                {
                    switch (tile.Tile.TileType)
                    {
                        case TileType.Cannon:
                        case TileType.Mortar:
                        case TileType.Tower:
                            if (!towers.Contains(tile.Tile))
                                towers.Add(tile.Tile);
                            break;
                        default:
                            if (!tiles.Contains(tile.Tile))
                                tiles.Add(tile.Tile);
                            break;

                    }
                }
            }
        }
    }


    private void OnEnable()
    {
        GameManager.OnTurnStateChange += OnTurnStateChange;
    }
    private void OnDisable()
    {
        GameManager.OnTurnStateChange -= OnTurnStateChange;
    }
    #endregion
}

[System.Serializable]
public struct UnlockTile
{
    public SOTileData Tile;
    public int UnlockDay;
}
