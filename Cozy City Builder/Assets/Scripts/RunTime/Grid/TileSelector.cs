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
    [SerializeField] private SOTileData[] pathTiles;

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

    public SOTileData GetPathTile(bool isRandom = false)
    {
        if (isRandom)
        {
            return pathTiles[Random.Range(0, pathTiles.Length)];
        }
        // total 3 type
        if (GameManager.Instance.DayCount == 1)
            return pathTiles[0];
        if (GameManager.Instance.DayCount == 2)
        {
            int rndNum = Random.Range(0, 100);
            return rndNum < 50 ? pathTiles[0] : pathTiles[1];
        }

        int rndNum2 = Random.Range(0, 100);
        if (rndNum2 < 33)
            return pathTiles[0];
        else if (rndNum2 >= 33 && rndNum2 < 67)
            return pathTiles[1];
        else
            return pathTiles[2];
    }
}
