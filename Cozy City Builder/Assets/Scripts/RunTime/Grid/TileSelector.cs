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
    //private List<SOTileData> _tilesPricelist;

    private void Start()
    {
        ResetList();
    }

    private void ResetList()
    {
        _openFreeTilesList = freeTiles.ToList();
        _openTilesPricelist = tilesWithPrice.ToList();
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
            _openTilesPricelist = tilesWithPrice.ToList();

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

    public SOTileData GetPathTile(bool isFree)
    {
        if (isFree)
            return pathTiles[1];
        return pathTiles[0];
    }

    public SOTileData[] GetStartTiles()
    {
        return startTiles;
    }
}
