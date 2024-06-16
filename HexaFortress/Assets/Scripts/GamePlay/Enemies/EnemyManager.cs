using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HexaFortress.Game;
using MyUtilities;
using UnityEngine;

namespace HexaFortress.GamePlay
{
    public class EnemyManager : SingletonMono<EnemyManager>
    {
        [Header("Enemies")]
        [SerializeField] private EnemyConfig[] tier1;
        [SerializeField] private EnemyConfig[] tier2;
        [Header("Bosses")]
        [SerializeField] private EnemyConfig boss1;
        [SerializeField] private EnemyConfig boss2;
        [Space(5)]
        [SerializeField] private int tier2Day;
        [SerializeField] private int tier3Day;
        [Space(10)]
        [SerializeField] private float spawnPointY;

        public float EnemyPosY => spawnPointY;

        private bool _debug = false;
        private bool _isSpawning;
        private Vector3 _spawnPos;
        private int _pathIndex;
        private Dictionary<int, List<Vector3>> _paths = new Dictionary<int, List<Vector3>>();
        private List<EnemyController> _enemyList = new List<EnemyController>();
        private EnemyFactory _enemyFactory = new();


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
            var time = new WaitForSeconds(1f);
            int count = GameModel.Instance.PlayerData.DayCount * GameModel.Instance.PlayerData.DayCount;
            // set path for each spawn point
            _paths.Clear();
            for (int i = 0; i < TileManager.Instance.EnemySpawnPoints.Count; i++)
            {
                var grid = GridManager.Instance.GetGridNode(TileManager.Instance.EnemySpawnPoints[i]);
                _paths[i] = GridManager.Instance.PathFinding.FindPath(grid.Position,
                    GameModel.Instance.CastleTile.MyHexNode.Position);
            }
            // spawn enemies
            while (count > 0)
            {
                for (int i = 0; i < TileManager.Instance.EnemySpawnPoints.Count; i++)
                {
                    yield return time;
                    _pathIndex = i;
                    _spawnPos = TileManager.Instance.EnemySpawnPoints[i];
                    _spawnPos.y = spawnPointY;
                    count -= SpawnFromTiers();
                    if (count <= 0)
                        break;
                }
            }
            SpawnBoss();
            _isSpawning = false;
            StartCoroutine(CheckAllEnemiesDead());
        }
        private int SpawnFromTiers()
        {
            if (GameModel.Instance.PlayerData.DayCount >= tier2Day)
            {
                int num = SpawnFromArray(tier2, 2f);
                if (num > 0)
                    return num;
            }
            int num1 = SpawnFromArray(tier1, 1f);
            return num1;
        }
        private int SpawnFromArray(EnemyConfig[] enemies, float multiplier)
        {
            int num = 0;
            for (int i = enemies.Length - 1; i >= 0; --i)
            {
                bool canSpawnThisEnemy = enemies[i].Level <= GameModel.Instance.PlayerData.DayCount
                                         && UnityEngine.Random.Range(0.0f, 1.0f) < (multiplier / enemies[i].Level);
                if (!canSpawnThisEnemy) continue;
                SpawnEnemy(enemies[i]);
                num = enemies[i].Level;
                break;
            }
            return num;
        }
        private void SpawnEnemy(EnemyConfig config)
        {
            // create enemy
            EnemyController newEnemyController = _enemyFactory.Create(config);
            // set path
            newEnemyController.transform.position = _spawnPos;
            newEnemyController.SetMovePosition(_paths[_pathIndex]);
            // Spawn animation
            Vector3 scale = newEnemyController.transform.localScale;
            newEnemyController.transform.localScale = Vector3.zero;
            newEnemyController.transform.DOScale(scale, 0.2f);
            // add to list
            _enemyList.Add(newEnemyController);
        }
        private void SpawnBoss()
        {
            if (GameModel.Instance.PlayerData.DayCount == tier2Day - 1)
                SpawnEnemy(boss1);
            else if (GameModel.Instance.PlayerData.DayCount == tier3Day - 1)
                SpawnEnemy(boss2);

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
        private void OnTurnStateChange(TurnStateChangeEvent evt)
        {
            if (evt.TurnState == TurnStates.EnemySpawnStart)
            {
                StartSpawn();
            }
        }
        private void OnGameStateChange(GameStateChangeEvent evt)
        {
            // when game over stop spawning
            if (evt.GameState == GameStates.GAMEOVER)
            {
                StopAllCoroutines();
            }
        }
        private void OnEnable()
        {
            EventManager.AddListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.AddListener<GameStateChangeEvent>(OnGameStateChange);
        }
        private void OnDisable()
        {
            EventManager.RemoveListener<TurnStateChangeEvent>(OnTurnStateChange);
            EventManager.RemoveListener<GameStateChangeEvent>(OnGameStateChange);
        }

        private void OnDrawGizmosSelected()
        {
            if (!_debug) return;
            foreach (var point in TileManager.Instance.EnemySpawnPoints)
            {
                Gizmos.DrawSphere(point, 0.3f);
            }
        }
    }
}

