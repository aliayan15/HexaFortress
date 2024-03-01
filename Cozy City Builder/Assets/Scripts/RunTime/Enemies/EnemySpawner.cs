using Managers;
using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : SingletonMono<EnemySpawner>
{
    [Header("Enemies")]
    [SerializeField] private Enemy[] tier1;
    [SerializeField] private Enemy[] tier2;
    [SerializeField] private Enemy[] tier3;
    [Header("Bosses")]
    [SerializeField] private Enemy boss1;
    [SerializeField] private Enemy boss2;
    [SerializeField] private Enemy boss3;
    [Space(5)]
    [SerializeField] private int tier2Day;
    [SerializeField] private int tier3Day;
    [Space(10)]
    [SerializeField] private float spawnPointY;

    private bool isDebug = false;
    private bool _isSpawning;
    private Vector3 _lastSpawnPos;
    private int _pathIndex;
    private Dictionary<int, List<Vector3>> _paths = new Dictionary<int, List<Vector3>>();
    private List<Enemy> _enemyList = new List<Enemy>();

    
    #region Spawn
    private void StartSpawn()
    {
        if (_isSpawning) return;
        _isSpawning = true;
        StartCoroutine(SpawnLoop());
    }
    private IEnumerator SpawnLoop()
    {
        if (TileManager.Instance.EnemySpawnPoints.Count == 0) yield break;
        // total count(not)
        var time = new WaitForSeconds(0.5f / TileManager.Instance.EnemySpawnPoints.Count);
        int count = GameManager.Instance.DayCount * GameManager.Instance.DayCount;
        // set path for each spawn point
        _paths.Clear();
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
        StartCoroutine(CheckAllEnemiesDead());
    }

    private int SpawnFromTiers()
    {
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
        return num1;
    }
    private int SpawnFromArray(Enemy[] enemies, float multiplier)
    {
        int num = 0;
        for (int i = enemies.Length - 1; i >= 0; --i)
        {
            bool canSpawnThisEnemy = enemies[i].Level <= GameManager.Instance.DayCount
                && UnityEngine.Random.Range(0.0f, 1f) < (multiplier / GameManager.Instance.DayCount);
            if (canSpawnThisEnemy)
            {
                SpawnEnemy(enemies[i]);
                num = Mathf.Max(Mathf.RoundToInt(enemies[i].Level / multiplier), 1);
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
        _enemyList.Add(newEnemy);
    }
    #endregion

    private IEnumerator CheckAllEnemiesDead()
    {
        var time = new WaitForSeconds(0.5f);
        while (_enemyList.Count > 0)
        {
            yield return time;
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (_enemyList[i] == null)
                {
                    _enemyList.RemoveAt(i);
                    i--;
                }
            }
        }
        OnAllEnemiesDead();
    }
    private void OnAllEnemiesDead()
    {
        GameManager.Instance.SetTurnState(TurnStates.TurnEnd);
    }

    private void OnTurnStateChange(TurnStates state)
    {
        if (state == TurnStates.EnemySpawnStart)
        {
            StartSpawn();
        }
        if (state == TurnStates.TurnEnd)
        {
            GameManager.Instance.IncreaseDay(1);
        }
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

