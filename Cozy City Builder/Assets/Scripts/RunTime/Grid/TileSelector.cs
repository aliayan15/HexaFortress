using Managers;
using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSelector : SingletonMono<TileSelector>
{
    [SerializeField] private SOTileData[] tilesWithPrice;
    [SerializeField] private SOTileData[] freeTiles;
    [Tooltip("1: Free, 0: Price")]
    [SerializeField] private SOTileData[] pathTiles;
    [Space(10)]
    [SerializeField] private SOTileData[] startTiles;
    [SerializeField] private SOTileData[] towers;

    private List<SOTileData> _openTilesPricelist = new List<SOTileData>();
    private List<SOTileData> _openFreeTilesList = new List<SOTileData>();
    private List<SOTileData> _openPathTilesList = new List<SOTileData>();
    //private List<SOTileData> _tilesPricelist;

    private void Start()
    {
        ResetList();
    }

    private void ResetList()
    {
        _openFreeTilesList = freeTiles.ToList();
        _openTilesPricelist = tilesWithPrice.ToList();
        ConstractPathList();
    }

    private void ConstractPathList()
    {
        _openPathTilesList.Clear();
        for (int i = 0; i < 4; i++)
        {
            _openPathTilesList.Add(pathTiles[0]);
        }
        for (int i = 0; i < 3; i++)
        {
            _openPathTilesList.Add(pathTiles[1]);
        }
        for (int i = 0; i < 2; i++)
        {
            _openPathTilesList.Add(pathTiles[2]);
        }
        _openPathTilesList.Shuffle();
    }

    public SOTileData GetTowerTile()
    {
        return towers[Random.Range(0, towers.Length)];
    }

    public SOTileData GetTileWithPrice()
    {
        int index = Random.Range(0, _openTilesPricelist.Count);
        var tile = _openTilesPricelist[index];
        _openTilesPricelist.RemoveAt(index);
        if (_openTilesPricelist.Count == 0)
        {
            _openTilesPricelist = tilesWithPrice.ToList();
            _openTilesPricelist.Shuffle();
        }
           
        return tile;
    }

    public SOTileData GetFreeTile()
    {
        int index = Random.Range(0, _openFreeTilesList.Count);
        var tile = _openFreeTilesList[index];
        _openFreeTilesList.RemoveAt(index);
        if (_openFreeTilesList.Count == 0)
            _openFreeTilesList = freeTiles.ToList();

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
        return startTiles;
    }
}
