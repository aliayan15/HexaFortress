using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileManager : SingletonMono<TileManager>
{
    public Dictionary<TileType, int> TileCount = new Dictionary<TileType, int>();
    public List<Vector3> EnemySpawnPoints = new List<Vector3>();

    public void AddEnemySpawnPoint(Vector3 spawnPoint)
    {
        if (!EnemySpawnPoints.Contains(spawnPoint))
            EnemySpawnPoints.Add(spawnPoint);
    }
    public void RemoveEnemySpawnPoint(Vector3 spawnPoint)
    {
        if (EnemySpawnPoints.Contains(spawnPoint))
            EnemySpawnPoints.Remove(spawnPoint);
    }

    public void AddNewTile(TileType type)
    {
        if (TileCount.ContainsKey(type))
            TileCount[type] = TileCount[type] + 1;
        else
            TileCount.Add(type, 1);
    }
    public void RemoveNewTile(TileType type)
    {
        if (TileCount.ContainsKey(type))
            TileCount[type] = TileCount[type] - 1;
    }

    public int GetTileCount(TileType type)
    {
        if (TileCount.ContainsKey(type))
            return TileCount[type];
        else return 0;
    }

    public int GetPriceOfTile(TileType type, int basePrice, int priceIncrease)
    {
        int count = GetTileCount(type);
        return basePrice + (count * priceIncrease);
    }
}
