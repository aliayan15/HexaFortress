using Managers;
using MyUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : SingletonMono<EnemySpawner>
{
    [Header("Enemies")]
    [SerializeField] private Enemy[] tier1;
    [SerializeField] private Enemy[] tier2;
    [SerializeField] private Enemy[] tier3;
    [SerializeField] private Enemy[] tier4;
    [Space(5)]
    [SerializeField] private int tier2Day;
    [SerializeField] private int tier3Day;
    [SerializeField] private int tier4Day;
    [Space(10)]
    [SerializeField] private float spawnPointY;

    private bool isDebug = false;
    private bool _isSpawning;
    private Vector3 _lastSpawnPos;
    private int _pathIndex;
    private Dictionary<int, List<Vector3>> _paths;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
            StartSpawn();
    }

    #region Spawn
    private void StartSpawn()
    {
        if (_isSpawning) return;
        _isSpawning = true;
        StartCoroutine(SpawnLoop());
    }
    private IEnumerator SpawnLoop()
    {
        // total count(not)
        var time = new WaitForSeconds(0.5f / TileManager.Instance.EnemySpawnPoints.Count);
        int count = GameManager.Instance.DayCount * GameManager.Instance.DayCount;
        // set path for each spawn point
        for (int i = 0; i < TileManager.Instance.EnemySpawnPoints.Count; i++)
        {
            var grid = GridManager.Instance.GetGridNode(TileManager.Instance.EnemySpawnPoints[i]);
            _paths[i] = GridManager.Instance.PathFinding.FindPath(grid.Position,
                        GridManager.Instance.PlayerCastle.MyHexNode.Position);
        }
        // spawn enemies
        while (count > 0)
        {
            for (int i = 0; i < TileManager.Instance.EnemySpawnPoints.Count; i++)
            {
                _pathIndex = i;
                _lastSpawnPos = TileManager.Instance.EnemySpawnPoints[i];

                count -= SpawnFromTiers();
                if (count <= 0)
                    break;
                yield return time;
            }

        }
        _isSpawning = false;
        GameManager.Instance.IncreaseDay(1);
    }

    private int SpawnFromTiers()
    {
        if (GameManager.Instance.DayCount >= tier4Day)
        {
            int num = SpawnFromArray(tier4, 4f);
            if (num > 0)
                return num;
        }
        if (GameManager.Instance.DayCount >= tier3Day)
        {
            int num = SpawnFromArray(tier3, 3f);
            if (num > 0)
                return num;
        }
        if (GameManager.Instance.DayCount >= tier2Day)
        {
            int num = SpawnFromArray(tier2, 2f);
            if (num > 0)
                return num;
        }
        int num1 = SpawnFromArray(tier1, 1f);
        if (num1 <= 0)
            Debug.LogError("Failed to Spawn minimum level enemy", this);
        return num1;
    }
    private int SpawnFromArray(Enemy[] enemies, float multiplier)
    {
        int num = 0;
        for (int i = enemies.Length - 1; i >= 0; --i)
        {
            bool canSpawnThisEnemy = enemies[i].Level <= GameManager.Instance.DayCount
                && UnityEngine.Random.Range(0.0f, 1f) < multiplier / GameManager.Instance.DayCount;
            if (canSpawnThisEnemy)
            {
                SpawnEnemy(enemies[i]);
                num = Mathf.Max(Mathf.RoundToInt(enemies[i].Level / multiplier), 1);
                Debug.Log(enemies[i].gameObject.name + " count: " + num);
                break;
            }
        }
        return num;
    }
    private void SpawnEnemy(Enemy enemy)
    {
        _lastSpawnPos.y = spawnPointY;
        Enemy newEnemy = Instantiate(enemy, _lastSpawnPos, Quaternion.identity);
        newEnemy.SetMovePosition(_paths[_pathIndex]);
    }
    #endregion

    private void OnTurnStateChange(TurnStates state)
    {
        // start spawning
    }

    private void OnGameStateChange(GameStates state)
    {
        // when game over stop spawning
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
        GameManager.OnTurnStateChange += OnTurnStateChange;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
        GameManager.OnTurnStateChange -= OnTurnStateChange;
    }

    private void OnDrawGizmos()
    {
        if (!isDebug) return;
        foreach (var point in TileManager.Instance.EnemySpawnPoints)
        {
            Gizmos.DrawSphere(point, 0.3f);
        }
    }
}

